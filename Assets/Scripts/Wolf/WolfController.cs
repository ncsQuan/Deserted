using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour
{
    public AudioSource wolfSound;
    public Transform player;
    public HealthBar healthBar;
    public HealthBarTest playerHealthBar;

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    public float maxHealth = 100;
    public float currentHealth;

    private bool isRunningOrAttacking = false;
    private bool isDead = false;

    public float lastTalk;
    public float targetTalk;
    float targetTalkBase = 1f;
    float targetTalkMaxRand = 1f;

    private float lastAttack;
    private float TIME_BETWEEN_ATTACKS = 1.5f;
    private float WOLF_ATTACK_DAMAGE = 5f;


    public void AnimateSit()
    {
        anim.SetBool("sit", true);
        anim.SetBool("run", false);
        anim.SetBool("attack", false);
        anim.SetBool("die", false);
    }

    public void AnimateRun()
    {
        anim.SetBool("sit", false);
        anim.SetBool("run", true);
        anim.SetBool("attack", false);
        anim.SetBool("die", false);
    }

    public void AnimateAttack()
    {
        anim.SetBool("sit", false);
        anim.SetBool("run", false);
        anim.SetBool("attack", true);
        anim.SetBool("die", false);
    }

    public void AnimateDie()
    {
        anim.SetBool("sit", false);
        anim.SetBool("run", false);
        anim.SetBool("attack", false);
        anim.SetBool("die", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        lastTalk = Time.timeSinceLevelLoad;

        setTargetTalk();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isDead)
        {
            AnimateDie();
        }
        else if (isRunningOrAttacking)
        {
            if (Vector3.Distance(player.position, this.transform.position) < 2)
            {
                AnimateAttack();
                navMeshAgent.SetDestination(transform.position);
                if (Time.timeSinceLevelLoad - lastAttack > TIME_BETWEEN_ATTACKS)
                {
                    lastAttack = Time.timeSinceLevelLoad;
                    playerHealthBar.TakeDamage(WOLF_ATTACK_DAMAGE);
                }
            }
            else
            {
                AnimateRun();
                navMeshAgent.SetDestination(player.position);
            }
            if (Time.timeSinceLevelLoad - lastTalk > targetTalk)
            {
                lastTalk = Time.timeSinceLevelLoad;
                wolfSound.Play();
                setTargetTalk();
            }


        }
        else
        {
            AnimateSit();
        }
    }

    void setTargetTalk()
    {

        targetTalk = targetTalkBase + Random.value * targetTalkMaxRand;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null && !isDead)
        {
            WolfAggro wc = c.attachedRigidbody.gameObject.GetComponent<WolfAggro>();
            if (wc != null)
            {
                AnimateRun();
                isRunningOrAttacking = true;
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null && !isDead)
        {
            WolfAggro wc = c.attachedRigidbody.gameObject.GetComponent<WolfAggro>();
            if (wc != null)
            {
                AnimateSit();
                isRunningOrAttacking = false;
            }
        }
        wolfSound.Stop();
    }

    public void takeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        healthBar.SetHealth(currentHealth);
        if (currentHealth == 0)
        {
            isDead = true;
            wolfSound.Stop();
        }
    }
}
