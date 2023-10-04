using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

//Code structure and format taken from https://github.gatech.edu/IMTC/CS4455_M1_Support/blob/master/Assets/Scripts/AppEvents/AudioEventManager.cs
public class AudioManager : MonoBehaviour
{

    [Header("Worldspace Audio Prefab")]

    [Description("Empty gameobject prefab that gets instantiated at a specific location in the game world and plays an audio")]
    public SpatialAudioEvent spatialAudioInstancePrefab;

    [Header("Always on audio")]
    public AudioClip backgroundMusic;
    public AudioClip waves;

    [SerializeField]
    private UnityAction noParameterEventListener;

    private void Awake()
    {
        noParameterEventListener = new UnityAction(noParameterEventHandler);
    } 

    // Subscribe to the event
    private void OnEnable()
    {
        EventManager.StartListening<NoParameterEvent>(noParameterEventHandler);
    }

    //Unsubscribe to the event
    private void OnDisable()
    {
        EventManager.StopListening<NoParameterEvent>(noParameterEventHandler);
    }

    #region Event Handlers
    // In this section you can include all the handlers for the events you're subscribed to
    void noParameterEventHandler()
    {
        //your event here
        return;
    }
    #endregion
}
