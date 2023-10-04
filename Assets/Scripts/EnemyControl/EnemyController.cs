using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    EnemyInterface enemyInterface;
    public LayerMask playerLayer;

    [NonSerialized]
    public Transform playerTransform;
    private Animator enemyAnim;
    private NavMeshAgent navMeshAgent;

    public Slider healthSlider;
    private void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        enemyInterface = GetComponent<EnemyInterface>();
        enemyAnim = GetComponent<Animator>();
        if (enemyInterface == null)
        {
            Debug.LogError("Enemy Interface not attached to enemy prefab");
        }

        healthSlider.maxValue = enemyInterface.getCurrentHealth();
        healthSlider.value = enemyInterface.getCurrentHealth();
    }

    public void UpdateHealth(float health)
    {
        healthSlider.value = health;
    }

    public void DealDamage() {
        EventManager.TriggerEvent<PlayerDamageEvent, float>(enemyInterface.getCurrentDamage());
    }

    public void HandleDeath()
    {
        GameObject drop = enemyInterface.enemySettings.drop;

        if (drop != null)
        {
            Instantiate(drop, transform.position, drop.transform.rotation);
        }

        Destroy(gameObject);
    }
}
