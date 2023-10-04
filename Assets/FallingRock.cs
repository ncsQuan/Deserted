using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tf;
    private Vector3 originalPos;
    private AudioSource source;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        originalPos = this.transform.position;
        source = GetComponent<AudioSource>();
        source.volume = 0.2f;
        



    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            rb.isKinematic = false;
            source = GetComponent<AudioSource>();
            if (count < 1)
            {
                source.Play();
                count += 1;
            }
            

        }
        //else
        //{
        //    rb.isKinematic = true;
        //    //this.transform.position = originalPos;
        //}
    }
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            EventManager.TriggerEvent<PlayerDamageEvent, float>(45);
        }
    }
}
