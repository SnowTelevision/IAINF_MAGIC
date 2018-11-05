using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// What happenes when a drone die
/// </summary>
public class DroneDie : EnemyDie
{
    public GameObject lightningAttackRangeTrigger; // The trigger represents the lightning's attack range

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnEnemyDie()
    {
        // Destroy the lightning attack range trigger when the enemy die
        Destroy(lightningAttackRangeTrigger);

        base.OnEnemyDie();
    }
}
