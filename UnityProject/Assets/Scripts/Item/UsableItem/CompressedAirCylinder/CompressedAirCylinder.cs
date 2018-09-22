using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functions for a compressed air cylinder
/// </summary>
public class CompressedAirCylinder : MonoBehaviour
{
    public float pushingForce; // How strong the cylinder will push the player's armTip

    public float defaultFirstArmSegmentConnectedMassScale;
    public bool isBeingUsed; // If the player is using it by holding down the trigger

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the compressed air cylinder hits an enemy unit while the player is using it
        if (isBeingUsed && collision.collider.GetComponent<EnemyDie>())
        {
            // Make the enemy fly away
            collision.collider.GetComponent<EnemyDie>().OnEnemyDie();
            // If it hits an engineer
            if (collision.collider.GetComponent<EngineerDie>())
            {
                collision.collider.GetComponent<EngineerDie>().OnEngineerDie();
            }
            // If it hits a drone
            if (collision.collider.GetComponent<DroneDie>())
            {

            }
            EnemyFlyAway(collision.gameObject, collision.impulse, collision.contacts[0].point);
        }
    }

    /// <summary>
    /// Make the enemy fly away if the player use it to collide on an enemy
    /// </summary>
    /// <param name="collidingEnemy"></param>
    /// <param name="collisionForce"></param>
    /// <param name="collisionPosition"></param>
    public void EnemyFlyAway(GameObject collidingEnemy, Vector3 collisionForce, Vector3 collisionPosition)
    {
        collidingEnemy.AddComponent<Rigidbody>();
        collidingEnemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //collidingEnemy.GetComponent<Rigidbody>().AddForceAtPosition(-collisionForce * 20000000f, collisionPosition, ForceMode.Impulse);
        collidingEnemy.GetComponent<Rigidbody>().AddForceAtPosition(-collisionForce * 20000000f + collisionForce.magnitude * Vector3.up * 10000000f, collisionPosition, ForceMode.Impulse);
    }

    /// <summary>
    /// Pushs the cylinder and the armTip that's holding it to its forward direction
    /// </summary>
    public void Propel()
    {
        GetComponent<ItemInfo>().holdingArmTip.GetComponentInParent<ControlArm_UsingPhysics>().jointConnectingBody.connectedMassScale = 1000;
        GetComponent<ItemInfo>().holdingArmTip.GetComponent<Rigidbody>().AddForce(transform.forward * pushingForce, ForceMode.Force);
        //GetComponent<Rigidbody>().AddForce(transform.forward * pushingForce, ForceMode.Force);
        isBeingUsed = true;
    }

    /// <summary>
    /// When the player stop using it
    /// </summary>
    public void Stop()
    {
        GetComponent<ItemInfo>().holdingArmTip.GetComponentInParent<ControlArm_UsingPhysics>().jointConnectingBody.connectedMassScale =
            defaultFirstArmSegmentConnectedMassScale;
        isBeingUsed = false;
    }

    /// <summary>
    /// What to do when the item is just picked up
    /// </summary>
    public void SetUpOnPickUp()
    {
        defaultFirstArmSegmentConnectedMassScale =
            GetComponent<ItemInfo>().holdingArmTip.GetComponentInParent<ControlArm_UsingPhysics>().jointConnectingBody.connectedMassScale;
    }
}
