using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarTest : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    private const float DecreasePerMinute = 60f;

    public HealthBar healthBar;
    public EnergyBar energyBarTest;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(5);
        }
        if (energyBarTest.GetEnergyValue() == 0)
        {
            decreaseHealthDueToNoEnergy();
        }*/
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void decreaseHealthDueToNoEnergy()
    {
        currentHealth = Mathf.Max(0, currentHealth - Time.deltaTime * DecreasePerMinute / 60f);
        healthBar.SetHealth(currentHealth);
    }
}
