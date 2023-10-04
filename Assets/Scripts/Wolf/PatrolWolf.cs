using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolWolf : MonoBehaviour
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


    public LayerMask PlayerLayer;
    public float sightRange, attackRange;
    private bool canSeePlayer, canAttackPlayer;
    private Vector3 walk;
    bool walkPoint;
    public float range;


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
        canSeePlayer = Physics.CheckSphere(transform.position, sightRange, PlayerLayer);
        canAttackPlayer = Physics.CheckSphere(transform.position, attackRange, PlayerLayer);
        if (isDead)
        {
            AnimateDie();
        }
        else if (canAttackPlayer && canSeePlayer)
        {
            navMeshAgent.SetDestination(transform.position);
            transform.LookAt(player);
            AnimateAttack();
            if (Time.timeSinceLevelLoad - lastAttack > TIME_BETWEEN_ATTACKS)
            {
                lastAttack = Time.timeSinceLevelLoad;
                Debug.Log("Taking damage");
                playerHealthBar.TakeDamage(WOLF_ATTACK_DAMAGE);
            }
            if (Time.timeSinceLevelLoad - lastTalk > targetTalk)
            {
                lastTalk = Time.timeSinceLevelLoad;
                wolfSound.Play();
                setTargetTalk();
            }
        }
        else if (!canAttackPlayer && !canSeePlayer)
        {
            patrol();
        }
        else
        {
            navMeshAgent.SetDestination(player.position);
            AnimateRun();
            if (Time.timeSinceLevelLoad - lastTalk > targetTalk)
            {
                lastTalk = Time.timeSinceLevelLoad;
                wolfSound.Play();
                setTargetTalk();
            }
        }

       /* if (Input.GetKeyDown("l"))
        {
            takeDamage(20);
        }*/

        if (isDead)
        {
            AnimateDie();
        }
        else if (isRunningOrAttacking)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 2)
            {
                Debug.Log("Attacking");
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
        Debug.Log("Wolf health: " + currentHealth);
        if (currentHealth <= 0)
        {
            isDead = true;
            wolfSound.Stop();
            AnimateDie();
        }
        Debug.Log("isDead: " + isDead);
    }
    private void patrol()
    {
        if (!walkPoint)
        {
            float temp = Random.Range(-range, range);
            float temp2 = Random.Range(-range, range);
            walk = new Vector3(transform.position.x + temp2, transform.position.y, transform.position.z + temp);
            walkPoint = true;


        }
        else if (walkPoint)
        {
            Debug.Log("Patroling");
            AnimateRun();
            navMeshAgent.SetDestination(walk);
        }

        Vector3 distance = transform.position - walk;
        if (distance.magnitude < 2f)
        {
            Debug.Log("Stopped Patroling");
            AnimateSit();
            walkPoint = false;
        }
    }
}
