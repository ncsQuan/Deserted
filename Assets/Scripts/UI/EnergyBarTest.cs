using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarTest : MonoBehaviour
{
    public float maxEnergy = 100;
    public float currentEnergy;

    private const float DecreasePerMinute = 60f;

    public EnergyBar energyBar;
    public HealthBarTest healthBarTest;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
        energyBar.SetMaxEnergy(maxEnergy);
    }

    // Update is called once per frame
    void Update()
    {
        currentEnergy = Mathf.Max(0, currentEnergy - Time.deltaTime * DecreasePerMinute / 60f);
        energyBar.SetEnergy(currentEnergy);

        if (Input.GetKeyDown(KeyCode.I))
        {
            IncreaseEnergy(10);
        }
    }

    void IncreaseEnergy(float increase)
    {
        currentEnergy = Mathf.Min(100, currentEnergy + increase);
        energyBar.SetEnergy(currentEnergy);
    }
}
