using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip ("Time to live in seconds")]
    [Range (1, 10)]
    public float timeToLive= 3;
    public float damage = 1;
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward*speed;
    }


    private void Update()
    {
        //Destroy the object after timeToLive has passed
        //and it hasn't collided
        Destroy(gameObject, timeToLive);
    }

    private void OnCollisionEnter(Collision other)
    {

        EnemyInterface enemyInterface = other.gameObject.GetComponent<EnemyInterface>();
        if (enemyInterface != null)
        {
            enemyInterface.modifyHealth(-(int)damage);
        }

        if (!other.gameObject.tag.Equals("Player"))
        {
            Destroy(gameObject);
        }
    }
}
