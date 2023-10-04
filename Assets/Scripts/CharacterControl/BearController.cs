using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BearController : MonoBehaviour
{
    private Animator anim;
    public Slider health;


    public void AnimateSleep()
    {
        anim.SetBool("attack", false);
        anim.SetBool("sleep", true);
    }

    public void AnimateAttack()
    {
        anim.SetBool("sleep", false);
        anim.SetBool("attack", true);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            BearAggro bc = c.attachedRigidbody.gameObject.GetComponent<BearAggro>();
            if (bc != null)
            {
                AnimateAttack();
                health.value = health.value - 20;
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            BearAggro bc = c.attachedRigidbody.gameObject.GetComponent<BearAggro>();
            if (bc != null)
            {
                AnimateSleep();
            }
        }
    }
}
