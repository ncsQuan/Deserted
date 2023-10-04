using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public class SpellCast
{
    public SpellScriptableObject spellObject;
    public float nextCastTime;
    public int ammountCollected;
}

public class SpellManager : MonoBehaviour
{
    private const float flareProjectileSpeed = 10f;

    #region Basic Attack Variables
    public GameObject basicAttackPrefab;
    private Transform attackStartingPoint;
    public float projectileSpeed;
    #endregion

    #region Jump Spell Variables
    private Rigidbody playerRb;
    [Range(400, 700)]
    public float jumpForce = 400f;
    #endregion

    #region TimeSpell Variables
    private bool timeStopped;
    [Header("Time Spell")]
    public LayerMask affectedObjects;
    [Range(0, 1)]
    [Description("Percentage of spell effect time relative to its cooldown")]
    public float relativeSpellDuration = 0.3f;
    private Collider[] stoppedColliders;
    #endregion

    #region Flare Spell Variables
    public GameObject flareProjectilePrefab;
    public GameObject flareCamera;
    #endregion

    #region Spell System
    //Keep track of the availabilty of each spell
    //since they're hosted as an enum, we'll have a true/false
    //value for each, depending on the index
    private Dictionary<Spells, SpellCast> availableSpells = new();
    private PlayerInterface playerInterface;
    #endregion

    #region Spell Related Events
    private UnityAction<SpellScriptableObject> SpellPickUpEventListener;
    #endregion

    private void Awake()
    {
        SpellPickUpEventListener = new UnityAction<SpellScriptableObject>(SpellPickUpEventHandler);
    }

    //Time Spell Details
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerInterface = GetComponent<PlayerInterface>();
        if (playerInterface == null)
        {
            Debug.LogError("ERROR: Player Interface component not attached to Player object");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BasicAttack(string side)
    {
        string searchString = "mixamorig:Hips/" +
            "mixamorig:Spine/mixamorig:Spine1/" +
            "mixamorig:Spine2/mixamorig:{0}Shoulder/" +
            "mixamorig:{0}Arm/mixamorig:{0}ForeArm/" +
            "mixamorig:{0}Hand" +
            "/mixamorig:{0}HandIndex1/" +
            "mixamorig:{0}HandIndex2/" +
            "mixamorig:{0}HandIndex3/" +
            "mixamorig:{0}HandIndex4";
        attackStartingPoint = transform.Find(string.Format(searchString, side));

        bool isAiming = GetComponent<PlayerController>().aim;
        Vector3 intercept = GetComponent<AimController>().interceptPosition;
        Vector3 aimDirection = (intercept - attackStartingPoint.position).normalized;

        Quaternion rotation = isAiming ? Quaternion.LookRotation(aimDirection, Vector3.up) : transform.rotation;
        Instantiate(basicAttackPrefab, attackStartingPoint.position, rotation);
    }

    public void CastFlare()
    {
        Transform flareStartingPoint = transform.Find("FlareSource");
        GameObject flare = Instantiate(flareProjectilePrefab, flareStartingPoint.position, transform.rotation);
        flare.GetComponent<Rigidbody>().AddForce(Vector3.up * flareProjectileSpeed, ForceMode.VelocityChange);
        StartCoroutine(followDelay(0.5f, flare));
    }

    private IEnumerator followDelay(float delayInSeconds, GameObject flare)
    {
        yield return new WaitForSeconds(delayInSeconds);
        flareCamera.GetComponent<CinemachineVirtualCamera>().LookAt = flare.transform;
        flareCamera.GetComponent<CinemachineVirtualCamera>().Follow = flare.transform;
        flareCamera.gameObject.SetActive(true);
    }

    public void Jump()
    {
        playerRb.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
        availableSpells[Spells.jump].nextCastTime = Time.fixedTime + availableSpells[Spells.jump].spellObject.coolDownInSeconds;

        int manaCost = availableSpells[Spells.jump].spellObject.manaCost;
        EventManager.TriggerEvent<SpellCastEvent, int>(-manaCost);
    }

    public void StopTime()
    {
        if (timeStopped) { return; };
        timeStopped = true;

        //Keep track of the stopped colliders to pass in to the Resume Time coroutine
        stoppedColliders = TimeSpell.StopTime(transform.position, affectedObjects);

        //Calculate mana cost to consume
        int manaCost = availableSpells[Spells.time].spellObject.manaCost;
        EventManager.TriggerEvent<SpellCastEvent, int>(-manaCost);

        //Calculate the cooldown of the spell
        float cooldown = availableSpells[Spells.time].spellObject.coolDownInSeconds;
        float nextCastTime = Time.fixedTime + cooldown;
        availableSpells[Spells.time].nextCastTime = nextCastTime;

        //Resume time after relativeSpellDuration percentage of the cooldown
        StartCoroutine(ResumeTimeAfterSeconds(cooldown * relativeSpellDuration));
    }

    IEnumerator ResumeTimeAfterSeconds(float resumeTime)
    {
        yield return new WaitForSeconds(resumeTime);
        TimeSpell.ResumeTime(stoppedColliders);
        timeStopped = false;
    }

    private void OnEnable()
    {
        EventManager.StartListening<SpellPickUpEvent, SpellScriptableObject>(SpellPickUpEventListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening<SpellPickUpEvent, SpellScriptableObject>(SpellPickUpEventListener);
    }

    //Whether or not the specific spell is available to cast
    public bool CanCast(Spells spell)
    {
        //check if the spell is an int
        if (Enum.IsDefined(typeof(Spells), spell))
        {
            bool spellExists = availableSpells.ContainsKey(spell);
            if (!spellExists){ return false;}

            //Special logic for flare spell
            if (spell.Equals(Spells.flare)) { 
                if (availableSpells[spell].ammountCollected < 5) { return false; }
            }

            float currentMana = playerInterface.getCurrentMana();
            bool notOnCooldown = availableSpells[spell].nextCastTime < Time.fixedTime;
            bool haveEnoughMana = availableSpells[spell].spellObject.manaCost <= currentMana;
            return notOnCooldown && haveEnoughMana; 
        }
        else
        {
            Debug.LogError("Incorrect spell integer passed");
            return false;
        }
    }

    #region Event Handlers
    //If we pick up a new spell, set the boolean to true in the available spells array
    //If more than one spell is picked up, keep a counter
    private void SpellPickUpEventHandler(SpellScriptableObject spell)
    {
        if (availableSpells.ContainsKey(spell.spellName)) {
            availableSpells[spell.spellName].ammountCollected += 1;
            return;
        }

        availableSpells.Add(spell.spellName, new SpellCast() { spellObject = spell, nextCastTime = Time.fixedTime });
    }
    #endregion
}
