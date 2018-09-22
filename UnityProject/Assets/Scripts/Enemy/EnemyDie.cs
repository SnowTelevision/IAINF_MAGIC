using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

/// <summary>
/// Super class for when an enemy dies
/// </summary>
public class EnemyDie : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// What happenes if an enemy die
    /// </summary>
    public void OnEnemyDie()
    {
        // Stop all behavior trees on this enemy
        foreach (BehaviorTree b in GetComponents<BehaviorTree>())
        {
            b.DisableBehavior();
        }
        // Destroy the NavMeshAgent in the enemy
        Destroy(GetComponent<NavMeshAgent>());

        Destroy(this);
    }
}
