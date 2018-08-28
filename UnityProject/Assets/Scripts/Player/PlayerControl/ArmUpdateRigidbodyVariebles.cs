using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates this arm segment's rigidbody's variebles such as drag and angular drag
/// </summary>
public class ArmUpdateRigidbodyVariebles : MonoBehaviour
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

    public Rigidbody thisRigidBody; // The rigidbody component that attaches to this arm
    public float armDragInWater; // The drag of the arm segments when they are in the water
    public float armAngularDragInWater; // The angular drag of the arm segments when they are in the water
    public float armTipConnectedJointMassScale; // The mass scale of spring joint of the last arm segment that is connected to the armTip when it is in the water
    public float armTipConnectedJointConnectedMassScale; // The connected mass scale of spring joint of the last arm segment that is connected to the armTip when it is in the water

    // Use this for initialization
    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDrag();
        UpdateJointMassScale();
        UpdateJointConnectedMassScale();

        //Test
        if (test)
        {
            //print(jointToBeUpdate.connectedMassScale);
            if (jointToBeUpdate.massScale / armTipJointMaximumMassScale > 0.5f)
            {
                print(jointToBeUpdate.massScale);
            }
        }
    }

    /// <summary>
    /// Updates the drag and angular drag for the rigidbody
    /// </summary>
    public void UpdateDrag()
    {
        if (updateDrag && armController.inWater) // If this arm is in the water then update the calculated drag for the arm
        {
            thisRigidBody.drag = armdefaultdraginwater * armController.armPhysicsMagnitude;
            thisRigidBody.angularDrag = armDefaultAngularDragInWater * armController.armPhysicsMagnitude;
        }
        else
        {

        }
    }

    /// <summary>
    /// Updates the mass scale and connected mass scale for the selected joint
    /// </summary>
    public void UpdateJointMassScale()
    {
        if (updateJointMassScale && armController.inWater)
        {
            jointToBeUpdate.massScale = Mathf.Clamp(armTipJointDefaultMassScale / armController.armPhysicsMagnitude,
                                                    armTipJointDefaultMassScale,
                                                    armTipJointMaximumMassScale);
        }
        else
        {

        }
    }

    /// <summary>
    /// Updates the connected mass scale and connected mass scale for the selected joint
    /// </summary>
    public void UpdateJointConnectedMassScale()
    {
        if (updateJointConnectedMassScale && armController.inWater)
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
