using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the wind strength of a windy area and give force to the player's arms if they entered the area
/// </summary>
public class WindyArea : MonoBehaviour
{
    public float windStrength; // The strength of the wind

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<ArmUseItem>()) // If this is an armTip
        {
            other.GetComponentInParent<ControlArm_UsingPhysics>().ApplyGlidingForceToBody(windStrength, transform.forward);
        }
    }
}
