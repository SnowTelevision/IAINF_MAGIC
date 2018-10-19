using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// What happenes when an engineer die
/// </summary>
public class EngineerDie : EnemyDie
{
    public GameObject carryingKey; // The key this engineer is carrying

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// What happenes when an engineer die
    /// </summary>
    public void OnEngineerDie()
    {
        Instantiate(carryingKey, transform.position - transform.forward - Vector3.up, transform.rotation);

        GetComponentInChildren<Animator>().speed = 0; // Stop the engineer animation when he dies
        Destroy(this);
    }
}
