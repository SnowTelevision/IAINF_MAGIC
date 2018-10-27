using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overload when the beam is hit by an energy ball
/// </summary>
public class EnergyBeam_Wide_Behavior : MonoBehaviour
{


    public bool isOverloaded; // Is this wide energy beam overloaded

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
        // If it is hit by an energy ball
        if (collision.collider.GetComponent<EnergyBallBehavior>())
        {
            // Make the emission brighter
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(GetComponent<MeshRenderer>().material.color, 2));

            isOverloaded = true;
        }
    }
}
