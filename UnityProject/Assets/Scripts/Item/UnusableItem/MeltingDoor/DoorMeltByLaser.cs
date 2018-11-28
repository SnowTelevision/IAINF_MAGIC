using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Let the door melt when the required number of lasers are shooting at it
/// </summary>
public class DoorMeltByLaser : MonoBehaviour
{
    public int lasersNeedToMelt; // How many laser beams is required to melt this door
    public Material meltDoorMaterial; // The material used for the melting door
    public MeshRenderer leftDoorMesh; // The mesh renderer for the left door 
    public MeshRenderer rightDoorMesh; // The mesh renderer for the right door 

    public List<GameObject> currentHittingLasers; // The lasers that are currently shooting at it
    public Material normalDoorMaterial; // The normal material for the door

    // Use this for initialization
    void Start()
    {
        normalDoorMaterial = leftDoorMesh.materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDoorMaterial();

        if (currentHittingLasers.Count >= lasersNeedToMelt)
        {
            gameObject.SetActive(false);
            this.enabled = false;
        }
    }


    public void UpdateDoorMaterial()
    {
        // If the door is being shoot by laser
        if (currentHittingLasers.Count > 0)
        {
            // If the material on the doors are not changed to melting material then change them
            if (leftDoorMesh.materials[1] != meltDoorMaterial)
            {
                leftDoorMesh.materials[1] = meltDoorMaterial;
                rightDoorMesh.materials[1] = meltDoorMaterial;
            }

            // Set the door melting material emission intensity
            leftDoorMesh.materials[1].SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(GetComponent<MeshRenderer>().material.color, currentHittingLasers.Count));
            rightDoorMesh.materials[1].SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(GetComponent<MeshRenderer>().material.color, currentHittingLasers.Count));
        }
        else
        {
            // If the material on the doors are melting material then change them back
            if (leftDoorMesh.materials[1] == meltDoorMaterial)
            {
                leftDoorMesh.materials[1] = normalDoorMaterial;
                rightDoorMesh.materials[1] = normalDoorMaterial;
            }
        }
    }
}
