using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellPickup : MonoBehaviour
{
    public SpellScriptableObject spell;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            EventManager.TriggerEvent<SpellPickUpEvent, SpellScriptableObject>(spell);
            GameObject.Find("UI Manager").GetComponent<UIManager>().spellPickUpEventHandler(spell); //Using this becuase of issue with events
            Destroy(gameObject);
        }
    }
}