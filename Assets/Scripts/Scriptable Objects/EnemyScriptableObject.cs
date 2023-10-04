using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="ScriptableObjects/Enemy", order =1)]
public class EnemyScriptableObject : ScriptableObject {
    public float damage;
    public float health;
    public float speed;
    public float angularSpeed;
    public float acceleration;
    public float stoppingDistance;
    [Header("Object to drop on death")]
    public GameObject drop;
}
