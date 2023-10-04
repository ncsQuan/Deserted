using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarTest : MonoBehaviour
{
    public float maxMana = 100;
    public float currentMana;

    private const float ManaIncreasePerMinute = 50f;

    public ManaBar manaBar;

    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        currentMana = Mathf.Min(maxMana, currentMana + Time.deltaTime * ManaIncreasePerMinute / 60f);
        manaBar.SetMana(currentMana);
        if (Input.GetKeyDown(KeyCode.M))
        {
            DecreaseMana(10);
        }
    }

    void DecreaseMana(float decrease)
    {
        currentMana = Mathf.Max(0, currentMana - decrease);
        manaBar.SetMana(currentMana);
    }
}
