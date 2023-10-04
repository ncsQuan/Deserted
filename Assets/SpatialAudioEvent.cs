using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatialAudioEvent : MonoBehaviour
{
    public AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
       source = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying) {
            Destroy(gameObject);
        }
    }
}
