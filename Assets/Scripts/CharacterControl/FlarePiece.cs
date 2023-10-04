using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlarePiece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            PlayerController pc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {

                pc.collectFlarePiece();
                //EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
                Destroy(this.gameObject);
            }
        }

    }
}
