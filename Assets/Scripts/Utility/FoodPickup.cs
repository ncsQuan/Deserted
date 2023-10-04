using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FoodPickup : MonoBehaviour
{

    public float energyRestored;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            EventManager.TriggerEvent<FoodPickupEvent, float>(energyRestored);
            Destroy(gameObject);
        }
    }
}
