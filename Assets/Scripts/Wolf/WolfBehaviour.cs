using UnityEngine;
using UnityEngine.AI;


public enum AIState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Death
}

public class WolfBehaviour : MonoBehaviour
{
    #region Patrolling Variables
    public float patrolRadius;
    #endregion

    #region AI Variables
    NavMeshAgent aiAgent;
    Animator animController;
    public AIState aiState;
    public LayerMask playerLayer;
    public LayerMask visibleLayers;
    public float visibilityRadius;
    private Vector3 intialPosition;

    [Tooltip("Every how many seconds does the agent change direction")]
    float minimumStayTime = 5f;
    float maximumStayTime = 10f;
    float timeForChangeInTarget = 0.0f;
    public float attackRange;
    #endregion

    public float distanceToPlayer = Mathf.Infinity;

    private EnemyController controller;
    // Start is called before the first frame update

    private void Awake()
    {
        animController = GetComponent<Animator>();
        if (animController == null)
        {
            Debug.LogError("Animator component not present");
        }
    }
    void Start()
    {
        intialPosition = transform.position;
        aiAgent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();

        if (aiAgent == null) {
            Debug.LogError("Nav Mesh Agent not found in the Enemy object");
        }

    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = updateDistanceToPlayer();
        switch (aiState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.Attack:
                Attack();
                break;
            case AIState.Death:
                Die();
                break;
        }

        reportCurrentSpeed();
    }

/*    private void FixedUpdate()
    {
        reportCurrentSpeed();
    }*/

    private bool reachedTarget()
    {
        bool reachedTarget = aiAgent.remainingDistance < 0.5f
                            && !aiAgent.pathPending;

        return reachedTarget;
    }

    private void reportCurrentSpeed() {
        animController.SetFloat("speed", aiAgent.velocity.magnitude);
    }

    private void Patrol() {
        //Transition condition

        if (distanceToPlayer < Mathf.Infinity)
        {
            aiState = AIState.Chase;
            return;
        } else if(distanceToPlayer <= attackRange)
        {
            aiState = AIState.Attack;
            return;
        }

        if (!reachedTarget()) {
            timeForChangeInTarget = Time.time + Random.Range(minimumStayTime, maximumStayTime) ;
            return; 
        }

        if (Time.time < timeForChangeInTarget){return;}

        Vector3 nextRandomTarget = generateRandomDestination(intialPosition);
        aiAgent.SetDestination(nextRandomTarget);
        timeForChangeInTarget = Time.time + Random.Range(minimumStayTime, maximumStayTime);
    }

    private Vector3 generateRandomDestination(Vector3 currentPosition)
    {
        Vector3 randomPointInSphere = Random.insideUnitSphere * patrolRadius;
        randomPointInSphere.y = currentPosition.y;

        randomPointInSphere += currentPosition;

        NavMeshHit hitQuery;
        NavMesh.SamplePosition(randomPointInSphere, out hitQuery, patrolRadius, visibleLayers);
        Vector3 nextTarget = hitQuery.position;
        
        return nextTarget;
    }

    private void Chase() {
        //TransitionConditions
        if (distanceToPlayer == Mathf.Infinity)
        {
            aiState = AIState.Patrol;
        } else if(distanceToPlayer <= attackRange)
        {
            aiState = AIState.Attack;
        }

        aiAgent.SetDestination(controller.playerTransform.position);
    }

    private void Attack() {
        if (distanceToPlayer < Mathf.Infinity && distanceToPlayer >= attackRange)
        {
            animController.SetBool("attacking", false);
            aiState = AIState.Chase;
            return;
        } else if (distanceToPlayer == Mathf.Infinity)
        {
            animController.SetBool("attacking", false);
            aiState = AIState.Patrol;
            return;
        }

        animController.SetBool("attacking", true);
    }

    private void Die()
    {
        animController.SetBool("attacking", false);
        aiAgent.ResetPath();
        animController.SetBool("die", true);
        return;
    }

    /// <summary>
    /// Provides distance to player if the player is in range
    /// otherwise returns Mathf.inf
    /// </summary>
    private float updateDistanceToPlayer() {
        bool playerInRange = Physics.CheckSphere(transform.position, visibilityRadius, playerLayer);
        
        if (!playerInRange) { return Mathf.Infinity; }

        RaycastHit hit;
        Vector3 higherTransform = new Vector3(transform.position.x, 5f, transform.position.z);
        Vector3 direction = (controller.playerTransform.position - higherTransform).normalized;
        bool rayCasthit = Physics.Raycast(transform.position, direction,
                            out hit, visibilityRadius,
                            visibleLayers);
        if(rayCasthit) {
            return Mathf.Infinity;
        }

        return Mathf.Clamp(Vector3.Distance(transform.position, controller.playerTransform.position), 0.3f, Mathf.Infinity);
    }
}
