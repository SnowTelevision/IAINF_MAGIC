using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

/// <summary>
/// What happenes when an engineer die
/// </summary>
public class EngineerDie : EnemyDie
{
    public GameObject carryingKey; // The key this engineer is carrying

    public bool isDying; // Is the engineer dying

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Override the base OnEnemyDie
    /// </summary>
    public override void OnEnemyDie()
    {
        // Stop all behavior trees on this enemy
        foreach (BehaviorTree b in GetComponents<BehaviorTree>())
        {
            b.DisableBehavior();
        }
        // Destroy the NavMeshAgent in the enemy
        Destroy(GetComponent<NavMeshAgent>());
    }

    /// <summary>
    /// What happenes when an engineer is hit by the player
    /// </summary>
    public void OnEngineerBeenHit()
    {
        isDying = true; // Start the dying event

        //GetComponentInChildren<Animator>().speed = 0; // Stop the engineer animation when he dies
        //Destroy(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the engineer is dying
        if (isDying)
        {
            // If the engineer hit ground
            if (collision.collider.name == "L1_RM5_ground 1" ||
                collision.collider.name == "L1_RM5_ground_glass1" ||
                collision.collider.name == "L1_RM5_ground_glass2")
            {
                EngineerHitGround();
            }
        }
    }

    /// <summary>
    /// When the engineer hit the ground
    /// </summary>
    public void EngineerHitGround()
    {
        GetComponentInChildren<Animator>().SetBool("PlayDead", true); // Play dying animation

        Instantiate(carryingKey, transform.position - transform.forward - Vector3.up, transform.rotation); // Instantiate key

        GetComponent<Rigidbody>().isKinematic = true; // Stop the engineer from moving
        Destroy(GetComponent<CapsuleCollider>()); // Destroy engineer's collider

        Destroy(this);
    }
}
