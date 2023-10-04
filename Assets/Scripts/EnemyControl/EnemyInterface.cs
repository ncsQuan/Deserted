using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterface : MonoBehaviour
{
    public EnemyScriptableObject enemySettings;

    #region Enemy Variables
    private float _maxDamage;
    private float _maxHealth;
    private float _currentHealth;
    private float _currentDamage;
    #endregion

    private void Awake()
    {
        _maxDamage = enemySettings.damage;
        _maxHealth = enemySettings.health;
        _currentDamage = _maxDamage;
        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Similar to modifyHealth.
    /// Pass positive/negative values to add/subtract from current
    /// enemy damage values.
    /// </summary>
    public void modifyDamage(float damage) {
        //Ensure we can't debuff past negative damage
        _currentDamage = Mathf.Clamp(_currentDamage + damage, 0, _maxDamage);
    }

    /// <summary>
    /// Adds value passed to enemy health value.
    /// Can be negative or positive to decrease/increase
    /// current health
    /// </summary>
    public void modifyHealth(int healthValue)
    {
        
        _currentHealth = Mathf.Clamp(_currentHealth + healthValue, 0, _maxHealth);
        GetComponent<EnemyController>().UpdateHealth(_currentHealth);

        if (_currentHealth == 0)
        {
            GetComponent<WolfBehaviour>().aiState = AIState.Death;
        }
    }

    public float getCurrentDamage()
    {
        return _currentDamage;
    }
    public float getCurrentHealth()
    {
        return _currentHealth;
    }
}
