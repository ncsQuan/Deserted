using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlareController : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeToLive;
    private void Start()
    {

        StartCoroutine(triggerEventAfterSeconds(timeToLive));
    }

    IEnumerator triggerEventAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        EventManager.TriggerEvent<PlayerWinEvent>();
    }
}
