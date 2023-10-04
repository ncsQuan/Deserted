using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private bool isDead;
    public GameObject Player;

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        SetHealth(maxHealth);
        fill.color = gradient.Evaluate(1);
    }
    public void SetHealth(float health)
    {
        Debug.Log("Taking damage");
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void Update()
    {
       
    }
}
