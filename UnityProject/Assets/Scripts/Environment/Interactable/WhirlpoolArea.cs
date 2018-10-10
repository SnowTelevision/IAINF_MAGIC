using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulating a whirlpool area that simulate whirlpool physics on the objects that enters the area
/// </summary>
public class WhirlpoolArea : MonoBehaviour
{
    public float radius; // The radius of the whirlpool
    public bool clockwise; // Does this whirlpool spin clockwise or counter-clockwise
    public float maximumLinearForce; // What's the maximum force this whirlpool can push objects to spin around
    public float maximumCentripetalForce; // What's the maximum force this whirlpool can suck in objects
    public float linearForceExponent; // The exponent of the linear force relate to how close the object is to the center of the whirlpool
    public float centripetalForceExponent; // The exponent of the centripetal force relate to how close the object is to the center of the whirlpool

    public List<Rigidbody> enteredObjects; // The objects that enters the whirlpool
    public float linearForceDivisor; // The divisor of the linear force to slow down the force increase speed as the object move towards the center
    public float centripetalForceDivisor; // The divisor of the centripetal force to slow down the force increase speed as the object move towards the center

    // Use this for initialization
    void Start()
    {
        // Set up the whirlpool radius
        GetComponent<SphereCollider>().radius = radius;
        // Set up the force divisors
        SolveForceDivisor();
    }

    // Update is called once per frame
    void Update()
    {
        // Apply the whirlpool force for each object that is in its area
        foreach (Rigidbody r in enteredObjects)
        {
            ApplyForce(r);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add the entered rigidbody if it is not in the affected list
        if (!enteredObjects.Contains(other.GetComponent<Rigidbody>()))
        {
            enteredObjects.Add(other.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the entered rigidbody if it is in the affected list
        if (enteredObjects.Contains(other.GetComponent<Rigidbody>()))
        {
            enteredObjects.Remove(other.GetComponent<Rigidbody>());
        }
    }

    /// <summary>
    /// Apply force to an object in the whirlpool
    /// </summary>
    /// <param name="affectedObject"></param>
    public void ApplyForce(Rigidbody affectedObject)
    {
        // Apply linear force
        affectedObject.AddForce(CalculateLinearForce(affectedObject.transform.position - transform.position), ForceMode.Force);
        // Apply centripetal force
        affectedObject.AddForce(CalculateCentripetalForce(affectedObject.transform.position - transform.position), ForceMode.Force);
    }

    /// <summary>
    /// Calculate how much linear force should be applied to the object depends on its position
    /// </summary>
    /// <param name="objectRelativePosition"></param>
    /// <returns></returns>
    public Vector3 CalculateLinearForce(Vector3 objectRelativePosition)
    {
        Vector3 linearForce = Vector3.zero;
        // Get the distance of the object to the center of this whirlpool
        float objectDistanceToCenter = Vector3.Magnitude(objectRelativePosition);
        // Get the whirlpool radius - distance
        float objectDistanceToEdge = Mathf.Clamp((radius - objectDistanceToCenter), 0, radius);
        // Get the centripetal force strength
        float forceStrength = Mathf.Pow(objectDistanceToEdge, linearForceExponent) / linearForceDivisor;
        // Determine if the whirlpool is going clockwise or counter-clockwise (1 is clockwise, -1 is counter-clockwise)
        float whirlpoolDirection = 0;
        if (clockwise)
        {
            whirlpoolDirection = 1;
        }
        else
        {
            whirlpoolDirection = -1;
        }
        // Calculate the x and z component of the tangent horizontal velocity
        float linearForceX = forceStrength / objectDistanceToCenter * Mathf.Abs(objectRelativePosition.z);
        float linearForceZ = forceStrength / objectDistanceToCenter * Mathf.Abs(objectRelativePosition.x);
        // Get the final converted tangent velocity
        linearForce.x =
            Mathf.Sign(objectRelativePosition.x) * Mathf.Sign(objectRelativePosition.x * objectRelativePosition.z) * whirlpoolDirection * linearForceX;
        linearForce.z =
            -Mathf.Sign(objectRelativePosition.z) * Mathf.Sign(objectRelativePosition.x * objectRelativePosition.z) * whirlpoolDirection * linearForceZ;

        return linearForce;
    }

    /// <summary>
    /// Calculate how much centripetal force should be applied to the object depends on its position
    /// </summary>
    /// <param name="objectRelativePosition"></param>
    /// <returns></returns>
    public Vector3 CalculateCentripetalForce(Vector3 objectRelativePosition)
    {
        Vector3 centripetalForce = Vector3.zero;
        // Get the distance of the object to the center of this whirlpool
        float objectDistanceToCenter = Vector3.Magnitude(objectRelativePosition);
        // Get the whirlpool radius - distance
        float objectDistanceToEdge = Mathf.Clamp((radius - objectDistanceToCenter), 0, radius);
        // Get the centripetal force strength
        float forceStrength = Mathf.Pow(objectDistanceToEdge, centripetalForceExponent) / centripetalForceDivisor;
        // Get the x and z value for the centripetal force
        centripetalForce.x = -forceStrength / objectDistanceToCenter * objectRelativePosition.x;
        centripetalForce.z = -forceStrength / objectDistanceToCenter * objectRelativePosition.z;

        return centripetalForce;
    }

    /// <summary>
    /// Calculate the divisors of the two forces based on the maximum force and the exponent
    /// </summary>
    public void SolveForceDivisor()
    {
        // Get the linear force divisor
        linearForceDivisor = Mathf.Pow(radius, linearForceExponent) / maximumLinearForce;
        // Get the centripetal force divisor
        centripetalForceDivisor = Mathf.Pow(radius, centripetalForceExponent) / maximumCentripetalForce;
    }
}
