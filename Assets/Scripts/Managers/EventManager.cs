using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// Code taken from https://github.gatech.edu/IMTC/CS4455_M1_Support/blob/master/Assets/Scripts/EventSystem/EventManager.cs by Dr. James Wilson
// Code commentary added by Alejandro Alonso for later study and reference
public class EventManager : MonoBehaviour
{

    private Dictionary<System.Type, UnityEventBase> eventDictionary;

    //Singleton class
    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType<EventManager>() as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("No EventManager gameobject found. One gameobject with the EventManager script must be present in the scene");
                } else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<System.Type, UnityEventBase>();
        }
    }

    #region Listener subscription functions
    //More details about C# generic functions https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-methods
    //The 'where' keyword is used enable more specialized operations on type parameters in methods

    //Template function to be used by listeners when looking for events with one argument
    public static void StartListening<Tbase> (UnityAction listener) where Tbase:UnityEvent, new()
    {
        UnityEventBase thisEvent = null;

        //https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.trygetvalue?view=net-8.0
        //Check if the event is already present in the EventManager dictionary. And if so, add it to the listener
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;
            
            if (e != null) { 
                e.AddListener(listener);
            }
            else
            {
                Debug.LogError(string.Format("EventManager.StartListening() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
            }
        } 
        //Else, add the event to the EventManager dictionary
        else
        {
            Tbase e = new Tbase();
            e.AddListener(listener);
            instance.eventDictionary.Add(typeof(Tbase), e);
        }
    }
   
    // Same functionality as StartListening. But handles UnityActions with one parameter
    public static void StartListening<Tbase, T0>(UnityAction<T0> listener) where Tbase : UnityEvent<T0>, new()
    {
        UnityEventBase thisEvent = null;

        //https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.trygetvalue?view=net-8.0
        //Check if the event is already present in the EventManager dictionary. And if so, add it to the listener
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.AddListener(listener);
            }
            else
            {
                Debug.LogError(string.Format("EventManager.StartListening() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
            }
        }
        //Else, add the event to the EventManager dictionary
        else
        {
            Tbase e = new Tbase();
            e.AddListener(listener);
            instance.eventDictionary.Add(typeof(Tbase), e);
        }
    }

    // Same functionality as StartListening. But handles UnityActions with two parameter

    public static void StartListening<Tbase, T0, T1>(UnityAction<T0, T1> listener) where Tbase : UnityEvent<T0, T1>, new()
    {
        UnityEventBase thisEvent = null;

        //https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.trygetvalue?view=net-8.0
        //Check if the event is already present in the EventManager dictionary. And if so, add it to the listener
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.AddListener(listener);
            }
            else
            {
                Debug.LogError(string.Format("EventManager.StartListening() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
            }
        }
        //Else, add the event to the EventManager dictionary
        else
        {
            Tbase e = new Tbase();
            e.AddListener(listener);
            instance.eventDictionary.Add(typeof(Tbase), e);
        }
    }

    // Same functionality as StartListening. But handles UnityActions with three parameter

    public static void StartListening<Tbase, T0, T1, T2>(UnityAction<T0, T1, T2> listener) where Tbase : UnityEvent<T0, T1, T2>, new()
    {
        UnityEventBase thisEvent = null;

        //https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.trygetvalue?view=net-8.0
        //Check if the event is already present in the EventManager dictionary. And if so, add it to the listener
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.AddListener(listener);
            }
            else
            {
                Debug.LogError(string.Format("EventManager.StartListening() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
            }
        }
        //Else, add the event to the EventManager dictionary
        else
        {
            Tbase e = new Tbase();
            e.AddListener(listener);
            instance.eventDictionary.Add(typeof(Tbase), e);
        }
    }

    // Same functionality as StartListening. But handles UnityActions with four parameter

    public static void StartListening<Tbase, T0, T1, T2, T3>(UnityAction<T0, T1, T2, T3> listener) where Tbase : UnityEvent<T0, T1, T2, T3>, new()
    {
        UnityEventBase thisEvent = null;

        //https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.trygetvalue?view=net-8.0
        //Check if the event is already present in the EventManager dictionary. And if so, add it to the listener
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.AddListener(listener);
            }
            else
            {
                Debug.LogError(string.Format("EventManager.StartListening() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
            }
        }
        //Else, add the event to the EventManager dictionary
        else
        {
            Tbase e = new Tbase();
            e.AddListener(listener);
            instance.eventDictionary.Add(typeof(Tbase), e);
        }
    }

    #endregion

    #region Listener unsubscribe function

    //Template function to be used by listeners to unsubscribe from an event
    //If events with more than 3 arguments are needed, simply add a new pair of StartListening and StopListening
    //functions that take the desired number of arguments
    public static void StopListening<Tbase> (UnityAction listener) where Tbase: UnityEvent
    {
        if (eventManager == null)
        {
            return;
        }

        UnityEventBase thisEvent = null;

        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;
            
            if (e != null)
            {
                e.RemoveListener(listener);
            }
        }
    }

    public static void StopListening<Tbase, T0>(UnityAction<T0> listener) where Tbase : UnityEvent<T0>
    {
        if (eventManager == null)
        {
            return;
        }

        UnityEventBase thisEvent = null;

        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.RemoveListener(listener);
            }
        }
    }

    public static void StopListening<Tbase, T0, T1>(UnityAction<T0, T1> listener) where Tbase : UnityEvent<T0, T1>
    {
        if (eventManager == null)
        {
            return;
        }

        UnityEventBase thisEvent = null;

        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.RemoveListener(listener);
            }
        }
    }

    public static void StopListening<Tbase, T0, T1, T2>(UnityAction<T0, T1, T2> listener) where Tbase : UnityEvent<T0, T1, T2>
    {
        if (eventManager == null)
        {
            return;
        }

        UnityEventBase thisEvent = null;

        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.RemoveListener(listener);
            }
        }
    }

    public static void StopListening<Tbase, T0, T1, T2, T3>(UnityAction<T0, T1, T2, T3> listener) where Tbase : UnityEvent<T0, T1, T2, T3>
    {
        if (eventManager == null)
        {
            return;
        }

        UnityEventBase thisEvent = null;

        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {
            Tbase e = (Tbase)thisEvent;

            if (e != null)
            {
                e.RemoveListener(listener);
            }
        }
    }

    #endregion

    #region Trigger functions

    public static void TriggerEvent<Tbase>() where Tbase : UnityEvent
    {
        UnityEventBase thisEvent = null;
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {

            Tbase e = thisEvent as Tbase;

            if (e != null)
                e.Invoke();
            else
                Debug.LogError(string.Format("EventManager.TriggerEvent() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
        }
    }

    public static void TriggerEvent<Tbase, T0>(T0 t0_obj) where Tbase : UnityEvent<T0>
    {
        UnityEventBase thisEvent = null;
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {

            Tbase e = thisEvent as Tbase;

            if (e != null)
                e.Invoke(t0_obj);
            else
                Debug.LogError(string.Format("EventManager.TriggerEvent() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
        }
    }

    public static void TriggerEvent<Tbase, T0, T1>(T0 t0_obj, T1 t1_obj) where Tbase : UnityEvent<T0, T1>
    {
        UnityEventBase thisEvent = null;
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {

            Tbase e = thisEvent as Tbase;

            if (e != null)
                e.Invoke(t0_obj, t1_obj);
            else
                Debug.LogError(string.Format("EventManager.TriggerEvent() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
        }
    }

    public static void TriggerEvent<Tbase, T0, T1, T2>(T0 t0_obj, T1 t1_obj, T2 t2_obj) where Tbase : UnityEvent<T0, T1, T2>
    {
        UnityEventBase thisEvent = null;
        if (instance.eventDictionary.TryGetValue(typeof(Tbase), out thisEvent))
        {

            Tbase e = thisEvent as Tbase;

            if (e != null)
                e.Invoke(t0_obj, t1_obj, t2_obj);
            else
                Debug.LogError(string.Format("EventManager.TriggerEvent() FAILED!. Event type {0} could not be accessed. ", typeof(Tbase).ToString()));
        }
    }
    #endregion
}
