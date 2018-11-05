using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the mechanical arm (either portable or stationary) like how the player controls the actual tentacle
/// </summary>
public class MechanicalArm : MonoBehaviour
{
    public float maximumArmReach; // How far the mechanical arm's hand can reach
    public float maximumArmLength; // How long the arm can extend
    public float minimumArmLength; // How short the arm can retract
    public float armMovingSpeed; // How fast can the arm hand move
    public Transform armHand; // The mechanical arm's hand
    public Transform arm; // The arm
    public Transform armHandLookAtTransform; // The transform the arm hand should look at
    public GameObject armCollisionDetectionTrigger; // The trigger that detects any potential collision to the arm
    public GameObject armHandCollisionDetectionTrigger; // The trigger that detects any potential collision to the arm hand
    //public float armLengthScalingMagnitude; // The multiplier to calculate how long the arm should extend based on the armHand's distance

    public ControlArm_UsingPhysics currentAccessingArm; // The arm (joystick) that is currently controlling it
    public float armHandLocalHeight; // The default local height of the arm hand
    public Vector3 targetPosition; // Arm hand's target position

    // Use this for initialization
    void Start()
    {
        // Set the local height for the arm hand
        armHandLocalHeight = armHand.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        //// 
        //if(currentAccessingArm == null)
        //{
        //    targetPosition = transform.position;
        //    MoveArm();
        //}

        // Lock the arm's y position when it is out
        //LockArmHandY();
        //UpdateRigidbodyConstraints();

        // Make sure the arm is always facing outward
        CorrectArmRotation();

        // Stretch the arm
        StretchArm();
    }

    /// <summary>
    /// Locks the arm hand's local y position to 0
    /// </summary>
    public void LockArmHandY()
    {
        if (arm.localScale.z > 1.9f)
        {
            armHand.localPosition = new Vector3(armHand.localPosition.x, 0, armHand.localPosition.z);
        }
    }

    /// <summary>
    /// Locks the arm and arm hand's y position when it is stretched out of the base
    /// </summary>
    public void UpdateRigidbodyConstraints()
    {
        if (arm.localScale.z > 1.9f)
        {
            armHand.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            armHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    /// <summary>
    /// Make sure the arm is always facing outward
    /// </summary>
    public void CorrectArmRotation()
    {
        armHand.LookAt(armHandLookAtTransform, Vector3.up);
        arm.LookAt(armHand, Vector3.up);
    }

    /// <summary>
    /// Adjust the arm to proper length
    /// </summary>
    public void StretchArm()
    {
        Vector3 armTargetScale = arm.localScale;
        armTargetScale.z = 
            minimumArmLength + (maximumArmLength - minimumArmLength) * 
            Vector3.Distance(transform.position + Vector3.up * armHandLocalHeight, armHand.position) / maximumArmReach;
        arm.localScale = armTargetScale;
    }

    /// <summary>
    /// Assign the current controlling arm
    /// </summary>
    public void SetupControllingArm()
    {
        currentAccessingArm = GetComponent<ItemInfo>().holdingArmTip.GetComponentInParent<ControlArm_UsingPhysics>();
        currentAccessingArm.isUsingMechanicalArm = true;
    }

    /// <summary>
    /// Resign the current controlling arm
    /// </summary>
    public void ResetControllingArm()
    {
        StopUsingArm();
        currentAccessingArm.isUsingMechanicalArm = false;
        currentAccessingArm = null;
    }

    /// <summary>
    /// When the player stop using the arm
    /// </summary>
    public void StopUsingArm()
    {
        //armHand.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    /// <summary>
    /// Moves the mechanical arm
    /// </summary>
    public void MoveArm()
    {
        // Get arm hand's target position
        targetPosition = transform.position + Vector3.up * armHandLocalHeight + currentAccessingArm.joystickPosition.normalized * //(Mathf.Clamp(currentAccessingArm.joystickPosition.magnitude, 0.01f, 1))
                         currentAccessingArm.joystickPosition.magnitude * maximumArmReach;

        if (DetectIfBlocked())
        {
            return;
        }

        // Get the target distance
        float targetDistance = Vector3.Magnitude(targetPosition - armHand.position);

        // If the hand is close to the target position
        if (targetDistance <= 0.2f)
        {
            armHand.GetComponent<Rigidbody>().AddForce((targetPosition - armHand.position).normalized * 0.1f, ForceMode.Force);
        }
        else
        {
            armHand.GetComponent<Rigidbody>().AddForce((targetPosition - armHand.position).normalized * armMovingSpeed, ForceMode.Force);
        }
    }

    /// <summary>
    /// Detect if there is some non-movable object in the arm's moving direction
    /// </summary>
    /// <returns></returns>
    public bool DetectIfBlocked()
    {
        // If the arm is about to collide on some non-movable object
        if (armCollisionDetectionTrigger.GetComponent<DetectCollision>().isEnteringCollider)
        {
            GameObject armCollidingObject = armCollisionDetectionTrigger.GetComponent<DetectCollision>().enteringCollider;

            if (!armCollidingObject.GetComponent<Rigidbody>() || armCollidingObject.GetComponent<Rigidbody>().isKinematic)
            {
                // Stop the arm
                armHand.GetComponent<Rigidbody>().velocity = Vector3.zero;
                arm.GetComponent<Rigidbody>().velocity = Vector3.zero;

                // If the collision and the move direction is on the same side
                if (Mathf.Sign(Vector3.Cross((armHand.position - transform.position),
                                             (armCollisionDetectionTrigger.GetComponent<DetectCollision>().collidingPoint - transform.position)).y) ==
                    Mathf.Sign(Vector3.Cross((armHand.position - transform.position),
                                             (targetPosition - transform.position)).y))
                {
                    targetPosition = armHand.position;
                    return true;
                }
                // If the collision and the move direction is not on the same side
                else
                {
                    goto check_arm_hand;
                }
            }
            else
            {
                goto check_arm_hand;
            }
        }
        else
        {
            goto check_arm_hand;
        }

        check_arm_hand:
        //return false;

        // If the arm hand is about to collide on some non-movable object
        if (armHandCollisionDetectionTrigger.GetComponent<DetectCollision>().isEnteringCollider)
        {
            GameObject armCollidingObject = armHandCollisionDetectionTrigger.GetComponent<DetectCollision>().enteringCollider;

            if (!armCollidingObject.GetComponent<Rigidbody>() || armCollidingObject.GetComponent<Rigidbody>().isKinematic)
            {
                // Stop the arm
                armHand.GetComponent<Rigidbody>().velocity = Vector3.zero;
                arm.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //Debug.DrawRay(armHand.position, armHand.position - transform.position, Color.yellow, 2);
                ////Debug.DrawRay(transform.position, armHandCollisionDetectionTrigger.GetComponent<DetectCollision>().collidingPoint - transform.position, Color.red, 2);
                //Debug.DrawLine(transform.position, armHandCollisionDetectionTrigger.GetComponent<DetectCollision>().collidingPoint, Color.red, 2);
                //print(armHandCollisionDetectionTrigger.GetComponent<DetectCollision>().collidingPoint);
                //Debug.DrawRay(armHand.position,
                //    Vector3.Cross((armHand.position - transform.position),
                //                  (armHandCollisionDetectionTrigger.GetComponent<DetectCollision>().collidingPoint - transform.position)), Color.magenta, 2);
                //Debug.DrawRay(armHand.position, targetPosition - transform.position, Color.green, 2);
                //Debug.DrawRay(armHand.position,
                //    Vector3.Cross((armHand.position - transform.position),
                //                  (targetPosition - transform.position)), Color.blue, 2);

                // If the collision and the move direction is on the same side
                if (Mathf.Sign(Vector3.Cross((armHand.position - transform.position),
                                             (armHandCollisionDetectionTrigger.GetComponent<DetectCollision>().collidingPoint - transform.position)).y) ==
                    Mathf.Sign(Vector3.Cross((armHand.position - transform.position),
                                             (targetPosition - transform.position)).y))
                {
                    armHand.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    arm.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    targetPosition = armHand.position;
                    return true;
                }
                // If the collision and the move direction is not on the same side
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
