using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipBar : MonoBehaviour
{

    public Slider slider;

    public float gameTime = 0f;
    private static float shipTime = 50f;
    private float islandStart = shipTime * 0.8f;
    public bool island = false;


    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = shipTime;

    }

    // Update is called once per frame
    void Update()
    {

        float time = gameTime + Time.time;
        if(time > islandStart)
        {
            //Debug.Log("Ship in reach");
            island = true;
        }
        if(time >= shipTime)
        {
            slider.value = slider.minValue;
            gameTime -= shipTime;
            island = false;
            //Debug.Log("Ship out of reach");
        }

        else
        {
            slider.value = time;
        }

    }

}
