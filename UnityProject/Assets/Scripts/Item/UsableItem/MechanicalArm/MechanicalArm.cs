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
    //public float armLengthScalingMagnitude; // The multiplier to calculate how long the arm should extend based on the armHand's distance

    public ControlArm_UsingPhysics currentAccessingArm; // The arm (joystick) that is currently controlling it
    Vector3 targetPosition; // Arm hand's target position

    // Use this for initialization
    void Start()
    {

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

        // Make sure the arm is always facing outward
        armHand.LookAt(transform, Vector3.up);
        arm.LookAt(armHand, Vector3.up);

        // Stretch the arm
        Vector3 armTargetScale = arm.localScale;
        armTargetScale.z = minimumArmLength + (maximumArmLength - minimumArmLength) * Vector3.Distance(transform.position, armHand.position) / maximumArmReach;
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
        targetPosition = transform.position + currentAccessingArm.joystickPosition.normalized * //(Mathf.Clamp(currentAccessingArm.joystickPosition.magnitude, 0.01f, 1))
                         currentAccessingArm.joystickPosition.magnitude * maximumArmReach;
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
}
