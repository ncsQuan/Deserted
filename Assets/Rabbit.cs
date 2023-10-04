using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit : MonoBehaviour
{
    private NavMeshAgent agent;

    public GameObject player;

    public float distance = 4.0f;

    private Animator animator;

    public GameObject destination;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float temp = Vector3.Distance(transform.position, player.transform.position);
        Vector3 newPos;
        if (temp < distance)
        {
            Vector3 disToPlayer = transform.position - player.transform.position;
            if (destination!=null)
            {
                float temp2 = Vector3.Distance(transform.position, destination.transform.position);
                if (temp2 > 1)
                {
                    newPos = destination.transform.position - disToPlayer;
                }
                else
                {
                    newPos = destination.transform.position;
                }
            }
            else
            {
                
                newPos = transform.position + disToPlayer;

            }
            if (destination != null && Vector3.Distance(transform.position, destination.transform.position) > 2)
            {
                animator.SetBool("isRunning", true);
                Debug.Log(animator.GetBool("isRunning"));
                agent.SetDestination(newPos);
            }else if (destination == null)
            {
                animator.SetBool("isRunning", true);
                agent.SetDestination(newPos);
            }

            
            
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        
    }
}
