using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the basic behaviors for every enemy
/// </summary>
public class CoreEnemyBehaviors : MonoBehaviour
{
    public bool canMove;
    public bool onlyMoveForward; // Does the enemy only move forward or it can also 
    public float baseMoveSpeed;
    public bool canRotate;
    public float baseRotateSpeed;
    public bool canSeek; // Can the enemy seek and follow player
    public float distanceToPlayerToFallBack; // How close to the player the enemy can be when it starts to fall back
    public bool canAttack;
    public float baseAttackRange;

    public Transform playerBodyTrans; // The player's body's transform
    public Vector3 playerBodyToSelf; // The player body's position - transform.position

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerBodyToSelf = playerBodyTrans.position - transform.position;
    }


}
