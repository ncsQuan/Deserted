using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    protected float timer;
    public int delayAmount = 5; // Second count
    public int days = 0;
    public bool ship = false;
    private ShipBar shipBar;


    // Start is called before the first frame update
    void Start()
    {
        shipBar = GameObject.FindGameObjectWithTag("ShipBar").GetComponent<ShipBar>();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        timer += Time.deltaTime;
       
        if (timer >= delayAmount)
        {
            
            timer = 0f;
            days++; 
            //daysText.text = "Days: " + days;

            if (days % delayAmount == 0)
            {
                Debug.Log("Ship in reach");
                ship = true;
            }
            else
            {
                Debug.Log("Ship out of reach");
                ship = false;
            }

            
        }
        */
        if (shipBar.island)
        {
            //Debug.Log("Ship in reach");
            ship = true;
        }
        else
        {
            //Debug.Log("Ship out of reach");
            ship = false;
        }
    }
}
