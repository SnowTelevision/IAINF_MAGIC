using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functions for a compressed air cylinder
/// </summary>
public class CompressedAirCylinder : MonoBehaviour
{
    public float propelAcceleration; // How strong the cylinder will push the player's armTip

    public float defaultFirstArmSegmentConnectedMassScale;
    public bool isBeingUsed; // If the player is using it by holding down the trigger
    public bool isTurning; // Is the player using the cylinder to turn around
    public float lastPropelMultiplier; // The propel acceleration multiplier in the last frame

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
            EnemyFlyAway(collision.gameObject, collision.impulse, collision.contacts[0].point);

            collision.collider.GetComponent<EnemyDie>().OnEnemyDie();
            // If it hits an engineer
            if (collision.collider.GetComponent<EngineerDie>())
            {
                collision.collider.GetComponent<EngineerDie>().OnEngineerBeenHit();
            }
            // If it hits a drone
            if (collision.collider.GetComponent<DroneDie>())
            {

            }
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
        // Prevent engineer from fliping
        collidingEnemy.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //collidingEnemy.GetComponent<Rigidbody>().AddForceAtPosition(-collisionForce * 20000000f, collisionPosition, ForceMode.Impulse);
        collidingEnemy.GetComponent<Rigidbody>().AddForceAtPosition(
            -collisionForce * 20000000f + collisionForce.magnitude * Vector3.up * 10000000f, collisionPosition, ForceMode.Impulse);
    }

    /// <summary>
    /// Pushs the cylinder and the armTip that's holding it to its forward direction
    /// </summary>
    public void Propel()
    {
        GetComponent<ItemInfo>().holdingArmTip.GetComponentInParent<ControlArm_UsingPhysics>().jointConnectingBody.connectedMassScale = 1000;
        //GetComponent<ItemInfo>().holdingArmTip.GetComponent<Rigidbody>().AddForce(transform.forward * pushingForce, ForceMode.Force);

        //// Get the player body current velocity
        //Vector3 playerVelocity = PlayerInfo.sPlayerInfo.GetComponent<Rigidbody>().velocity;

        //// Check if the player is turning
        //if (Vector3.Angle(playerVelocity, transform.forward) > 90)
        //{
        //    isTurning = true;
        //}

        //// Get the propel acceleration multiplier
        //float propelMultiplier = Mathf.Cos(Vector3.Angle(playerVelocity, transform.forward) * Mathf.Deg2Rad * 0.5f);

        //if (isTurning && (propelMultiplier - lastPropelMultiplier) > 0.1f)
        //{
        //    propelMultiplier = lastPropelMultiplier + 0.3f * Time.fixedDeltaTime;
        //}
        //else
        //{
        //    propelMultiplier = Mathf.Cos(Vector3.Angle(playerVelocity, transform.forward) * Mathf.Deg2Rad * 0.5f);
        //}

        //// Apply propelling acceleration
        //GetComponent<ItemInfo>().holdingArmTip.GetComponent<Rigidbody>().AddForce(propelMultiplier * transform.forward * propelAcceleration, ForceMode.Acceleration);

        //lastPropelMultiplier = propelMultiplier;

        //// If the player stop turning
        //if (propelMultiplier > 0.8f)
        //{
        //    isTurning = false;
        //}

        // Apply propelling acceleration
        GetComponent<ItemInfo>().holdingArmTip.GetComponent<Rigidbody>().AddForce(transform.forward * propelAcceleration, ForceMode.Acceleration);

        //// Get the player body current velocity
        //Vector3 playerVelocity = PlayerInfo.sPlayerInfo.GetComponent<Rigidbody>().velocity;
        //// Get the difference between the propelling acceleration and the player body current velocity
        //Vector3 velocityDifference =
        //    (transform.forward * propelAcceleration) - (10 * playerVelocity / GetComponent<ItemInfo>().holdingArmTip.GetComponent<Rigidbody>().mass);
        //// Apply propelling acceleration
        //if (Vector3.Angle(transform.forward, velocityDifference) <= 90)
        //{
        //    GetComponent<ItemInfo>().holdingArmTip.GetComponent<Rigidbody>().AddForce(velocityDifference, ForceMode.Acceleration);
        //}
        //GetComponent<Rigidbody>().AddForce(transform.forward * pushingForce, ForceMode.Force);
        isBeingUsed = true;

        //// Test
        //print(propelMultiplier);
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
