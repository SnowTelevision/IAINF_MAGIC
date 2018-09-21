using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the compressed air cylinder (short as CAC) that are used in testing
/// </summary>
public class TestedCompressedAirCylinderBehavior : MonoBehaviour
{
    public float minFailForce; // What is the minimum force for a failed CAC
    public float maxFailForce; // What is the maximum force for a failed CAC
    public float minSucceedForce; // What is the minimum force for a succeeded CAC
    public float maxSucceedForce; // What is the maximum force for a succeeded CAC

    public float successRate; // How much percent of the CAC should success
    public float randomSuccessRate; // What is the randomed success rate to compare with the success rate
    public float randomFailForce; // What is the randomed fail force to be used in the test
    public float randomSucceedForce; // What is the randomed succeed force to be used in the test

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Shoot out the cylinder, if the parameter is 
    /// </summary>
    /// <param name="isSuccess"></param>
    public void ShootCylinder(float isSuccess)
    {
        // If this is a success run
        if (isSuccess >= (1 - successRate))
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * randomSucceedForce, ForceMode.Impulse);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * randomFailForce, ForceMode.Impulse);
        }
    }
}
