using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the procedure for testing compressed air cyclinder (short as CAC)
/// </summary>
public class CompressedAirCylinderTestingManager : MonoBehaviour
{
    public GameObject testingCompressedAirCylinder; // The testing version of the CAC
    public Vector3 compressedAirCylinderInstantiatePosition; // Where the CAC come out
    public Vector3 compressedAirCylinderInstantiateEuler; // The CAC's euler angles when it come out
    public float testInterval; // How much time is between each test
    public float successRate; // How much percent of the CAC should success

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateNewCompressedAirCylinder()
    {
        // Create new compressed air cylinder
        GameObject newCompressedAirCylinder = Instantiate(testingCompressedAirCylinder);
        // Set the success rate
        newCompressedAirCylinder.GetComponent<TestedCompressedAirCylinderBehavior>().successRate = successRate;
        // Set the position and rotation
        newCompressedAirCylinder.transform.position = compressedAirCylinderInstantiatePosition;
        newCompressedAirCylinder.transform.eulerAngles = compressedAirCylinderInstantiateEuler;
    }
}
