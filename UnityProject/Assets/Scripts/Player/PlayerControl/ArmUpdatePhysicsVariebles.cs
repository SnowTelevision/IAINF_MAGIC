using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates this arm segment's physics variebles such as drag and angular drag
/// </summary>
public class ArmUpdatePhysicsVariebles : MonoBehaviour
{
    public bool test; // Do we print test outputs
    public ControlArm_UsingPhysics armController; // The controller that controls the arm this segment belongs to
    public bool updateDrag; // Do we update the drag of the rigidbody
    public float armdefaultdraginwater; // The default drag of the arm when it is in the water
    public float armDefaultAngularDragInWater; // The default angular drag of the arm when it is in the water
    public bool updateJointMassScale; // Do we update the mass scale of the joint
    public bool updateJointConnectedMassScale; // Do we update the connected mass scale of the joint
    public SpringJoint jointToBeUpdate; // The joint that is going to be updated
    public float armTipJointDefaultMassScale;
    public float armTipJointMaximumMassScale; // The maximum mass scale of the joint
    public float armTipJointDefaultConnectedMassScale;
    public float armTipJointMaximumConnectedMassScale; // The maximum connected mass scale of the joint
    public bool updateConnectedAnchor; // Do we update the connected anchor of the joint
    public float armMaxConnectedAnchor; // The longest distance this arm segment's connected anchor position on the arm segment it is connected to can extend
    public float armMinConnectedAnchor; // The shortest distance this arm segment's connected anchor position on the arm segment it is connected to can extend
    public bool updateAnchor; // Do we update the anchor of the joint
    public float armMaxAnchor; // The longest distance this arm segment's anchor position on the arm segment it is connected to can extend
    public float armMinAnchor; // The shortest distance this arm segment's anchor position on the arm segment it is connected to can extend
    public float maxJointChangingSpeed; // How fast the joint's position can change (on a 0 to 1 scale, 1 means the joint can instantly changing from min to max)

    public Rigidbody thisRigidBody; // The rigidbody component that attaches to this arm
    public float armDragInWater; // The drag of the arm segments when they are in the water
    public float armAngularDragInWater; // The angular drag of the arm segments when they are in the water
    public float armTipConnectedJointMassScale; // The mass scale of spring joint of the last arm segment that is connected to the armTip when it is in the water
    public float armTipConnectedJointConnectedMassScale; // The connected mass scale of spring joint of the last arm segment that is connected to the armTip when it is in the water
    public float lastJoystickLength; // The joystick length in the last frame;
    public float lastConnectedAnchorPos; // The position of the connected anchor in the last frame
    public float lastAnchorPos; // The position of the anchor in the last frame

    // Test
    public float jointAnchorChangingSpeed; // The speed the joint's anchor's position changes

    // Use this for initialization
    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody>();
        if (jointToBeUpdate == null)
        {
            jointToBeUpdate = GetComponent<SpringJoint>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDrag();
        UpdateJointMassScale();
        UpdateJointConnectedMassScale();

        // Only update the joint positions if the arm is not using an item that is fixed (not movable)
        if (armController.armTip.GetComponent<ArmUseItem>().currentlyHoldingItem == null ||
            //(armController.armTip.GetComponent<ArmUseItem>().currentlyHoldingItem.GetComponent<ItemInfo>() &&
             !armController.armTip.GetComponent<ArmUseItem>().currentlyHoldingItem.GetComponent<ItemInfo>().fixedPosition)
        {
            UpdateJointConnectedAnchor();
            UpdateJointAnchor();
        }

        //Test
        if (test)
        {
            //if (armController.startedSwimming)
            {

                //if (jointToBeUpdate.connectedMassScale / armTipJointMaximumConnectedMassScale > 0.5f)
                //{
                //    print(jointToBeUpdate.connectedMassScale);
                //}
                //if (jointToBeUpdate.massScale / armTipJointMaximumMassScale > 0.5f)
                if (Time.time - armController.lastArmSwimmingTime > 2)
                {
                    print(jointToBeUpdate.massScale);
                }
            }
        }
    }

    /// <summary>
    /// Updates the drag and angular drag for the rigidbody
    /// </summary>
    public void UpdateDrag()
    {
        if (updateDrag) // If this arm is in the water then update the calculated drag for the arm
        {
            if (armController.inWater)
            {
                thisRigidBody.drag = armdefaultdraginwater * armController.armPhysicsMagnitude;
                thisRigidBody.angularDrag = armDefaultAngularDragInWater * armController.armPhysicsMagnitude;
            }
            else
            {

            }
        }
    }

    /// <summary>
    /// Updates the mass scale and connected mass scale for the selected joint
    /// </summary>
    public void UpdateJointMassScale()
    {
        if (updateJointMassScale)
        {
            if (armController.inWater)
            {
                jointToBeUpdate.massScale = Mathf.Clamp(armTipJointDefaultMassScale / armController.armPhysicsMagnitude,
                                                        armTipJointDefaultMassScale,
                                                        armTipJointMaximumMassScale);

                jointToBeUpdate.massScale /= Vector3.Magnitude(armController.armTip.position - armController.transform.position) / armController.armMaxLength;
            }
            else
            {
                //jointToBeUpdate.massScale = armTipJointDefaultMassScale /
                //                            //(Vector3.Magnitude(armController.armTip.position - armController.transform.position) / armController.armMaxLength);
                //                            Mathf.Clamp(armController.joyStickLength, 0.1f, 1);
            }
        }
    }

    /// <summary>
    /// Updates the connected mass scale and connected mass scale for the selected joint
    /// </summary>
    public void UpdateJointConnectedMassScale()
    {
        if (updateJointConnectedMassScale)
        {
            if (armController.inWater)
            {
                jointToBeUpdate.connectedMassScale = Mathf.Clamp(armTipJointDefaultConnectedMassScale / armController.armPhysicsMagnitude,
                                                             armTipJointDefaultConnectedMassScale,
                                                             armTipJointMaximumConnectedMassScale);
            }
            else
            {

            }
        }
    }

    /// <summary>
    /// Update the joint's connected anchor based on the length of the arm
    /// </summary>
    public void UpdateJointConnectedAnchor()
    {
        if (test)
        {
            //jointAnchorChangingSpeed = (armMaxConnectedAnchor - armMinConnectedAnchor) * (armController.joyStickLength - lastJoystickLength);
        }

        if (updateConnectedAnchor)
        {
            // Update arm's connected anchor's position (clamped at a maximum speed)
            GetComponent<SpringJoint>().connectedAnchor =
                Vector3.up * (lastConnectedAnchorPos +
                              Mathf.Clamp((armMinConnectedAnchor + (armMaxConnectedAnchor - armMinConnectedAnchor) * lastJoystickLength - lastConnectedAnchorPos),
                                          -maxJointChangingSpeed, maxJointChangingSpeed));

            lastConnectedAnchorPos = GetComponent<SpringJoint>().connectedAnchor.y;
            lastJoystickLength = armController.joyStickLength;
        }
    }

    /// <summary>
    /// Update the joint's anchor based on the length of the arm
    /// </summary>
    public void UpdateJointAnchor()
    {
        if (test)
        {
            //jointAnchorChangingSpeed = (armMaxConnectedAnchor - armMinConnectedAnchor) * (armController.joyStickLength - lastJoystickLength);
        }

        if (updateAnchor)
        {
            // Update arm's connected anchor's position (clamped at a maximum speed)
            GetComponent<SpringJoint>().anchor =
                Vector3.up * (lastAnchorPos +
                              Mathf.Clamp((armMaxAnchor + (armMinAnchor - armMaxAnchor) * lastJoystickLength - lastAnchorPos),
                                          -maxJointChangingSpeed, maxJointChangingSpeed));

            if (test)
            {
                //print(armMaxAnchor + (armMinAnchor - armMaxAnchor) * lastJoystickLength);
            }

            lastAnchorPos = GetComponent<SpringJoint>().anchor.y;
            lastJoystickLength = armController.joyStickLength;
        }
    }
}
