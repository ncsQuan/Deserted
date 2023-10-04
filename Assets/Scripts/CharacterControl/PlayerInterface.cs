using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{

    [Header("Player Stats")]
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int maxMana;
    [SerializeField]
    private int maxEnergy;

    #region Private members
    private int _health;
    public float _mana;
    public float _energy;
    #endregion

    private void Start()
    {
        _health = maxHealth;
        _mana = maxMana;
        _energy = maxEnergy;
    }

    #region Getters and Setters for stats
    public void restoreHealth(int healthRestored)
    {
        _health = Mathf.Clamp(_health + healthRestored, 0, maxHealth);
    }

    public void takeDamage(int damage)
    {

        _health = Mathf.Clamp(_health - damage, 0, maxHealth);

        if (_health == 0)
        {
            GetComponent<PlayerController>().reportDeath();
            //EventManager.TriggerEvent<PlayerDeathEvent>();
        }
    }

    public int getCurrentHealth()
    {
        return _health;
    }

    public void setMana(float mana)
    {
        _mana = Mathf.Clamp(_mana + mana, 0, maxMana);
        
        if (_mana == 0)
        {
            GetComponent<Animator>().SetBool("sprint", false);
        }
    }

    public float getCurrentMana() {
        return _mana;
    }

    public void setEnergy(float energy)
    {
        _energy = Mathf.Clamp(_energy + energy, 0, maxEnergy);

        if (_energy == 0)
        {
            GetComponent<PlayerController>().reportDeath();
            //EventManager.TriggerEvent<PlayerDeathEvent>();
        }
    }

    public float getCurrentEnergy()
    {
        return _energy;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getMaxMana() { return maxMana;}

    public int getMaxEnergy() { return maxEnergy;}
    #endregion
}
