using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates this arm segment's rigidbody's variebles such as drag and angular drag
/// </summary>
public class ArmUpdateRigidbodyVariebles : MonoBehaviour
{
    public ControlArm_UsingPhysics armController; // The controller that controls the arm this segment belongs to

    public Rigidbody thisRigidBody; // The rigidbody component that attaches to this arm

    // Use this for initialization
    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDrag();
    }

    /// <summary>
    /// Updates the drag and angular drag for the rigidbody
    /// </summary>
    public void UpdateDrag()
    {
        if (armController.inWater) // If this arm is in the water then update the calculated drag for the arm
        {
            thisRigidBody.drag = armController.armDragInWater;
            thisRigidBody.angularDrag = armController.armAngularDragInWater;
        }
        else
        {

        }
    }
}
