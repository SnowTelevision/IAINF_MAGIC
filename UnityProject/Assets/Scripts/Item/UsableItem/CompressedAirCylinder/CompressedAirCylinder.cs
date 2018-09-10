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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Pushs the cylinder and the armTip that's holding it to its forward direction
    /// </summary>
    public void Propel()
    {
        GetComponent<ItemInfo>().holdingArmTip.GetComponentInParent<ControlArm_UsingPhysics>().jointConnectingBody.connectedMassScale = 1000;
        GetComponent<ItemInfo>().holdingArmTip.GetComponent<Rigidbody>().AddForce(transform.forward * pushingForce, ForceMode.Force);
    }

    /// <summary>
    /// When the player stop using it
    /// </summary>
    public void Stop()
    {
        GetComponent<ItemInfo>().holdingArmTip.GetComponentInParent<ControlArm_UsingPhysics>().jointConnectingBody.connectedMassScale = 
            defaultFirstArmSegmentConnectedMassScale;
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
