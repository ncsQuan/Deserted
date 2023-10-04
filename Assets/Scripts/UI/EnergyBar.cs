using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;

    public EnergyBar(float max)
    {
        slider.value = max;
    }

    public void SetMaxEnergy(float energy)
    {
        slider.maxValue = energy;
        slider.value = energy;
    }

    public void SetEnergy(float energy)
    {
        slider.value = energy;
    }

    public void UseEnergy(float energy)
    {
        float difference = slider.value - energy;
        SetEnergy(difference);
    }

    public float GetEnergyValue()
    {
        return slider.value;
    }




}