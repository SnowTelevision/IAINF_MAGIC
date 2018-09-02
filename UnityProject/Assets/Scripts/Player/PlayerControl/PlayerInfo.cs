using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the info that relates to the player's body
/// </summary>
public class PlayerInfo : MonoBehaviour
{
    public float maxHealth; // How much health the player can have
    public Transform gameCamera; // The transform of the camera that is looking at the player
    public float targetSideScrollOrbitRadius; // The radius of the player's orbit for the game's side-scrolling session

    public bool inWater; // Is the body in the water
    public static Transform sGameCamera; // The static reference to gameCamera
    public static PlayerInfo sPlayerInfo; // The static reference to PlayerInfo
    public static bool isSideScroller; // If the game is in side-scroller mode

    // Test
    public bool test;

    private void Awake()
    {
        // Assigning static references for variebles that has to be assigned in the editor thus cannot be static.
        sGameCamera = gameCamera;
        sPlayerInfo = this;
    }

    // Use this for initialization
    void Start()
    {
        // Test
        if (test)
        {
            isSideScroller = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isSideScroller) // If the game is in side-scrolling session
        {
            CorrectPlayerSideScrollingOrbit();
        }
    }

    /// <summary>
    /// Calculate the velocity that is tangent to the orbit that the player's body is currently at
    /// based on the player's current velocity. (assume the center of the orbit is (0, y, 0) in global)
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateTangentVelocity()
    {
        float currentVerticalSpeed = GetComponent<Rigidbody>().velocity.y; // The player's current speed along the y-axis
        // Calculate the player's current speed in the x-z plain
        Vector3 currentHorizontalVelocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
        float currentHorizontalSpeed = currentHorizontalVelocity.magnitude;
        // Determine if the player is going clockwise or counter-clockwise (1 is clockwise, -1 is counter-clockwise)
        float playerVelocityDirection = Mathf.Sign(Vector3.Cross(transform.position, GetComponent<Rigidbody>().velocity).y);
        // Calculate the x and z component of the tangent horizontal velocity
        float currentHorizontalDistance = new Vector3(transform.position.x, 0, transform.position.z).magnitude; // The horizontal distance from the player to the center
        float tangentHorizontalSpeedX = currentHorizontalSpeed / currentHorizontalDistance * Mathf.Abs(transform.position.z);
        float tangentHorizontalSpeedZ = currentHorizontalSpeed / currentHorizontalDistance * Mathf.Abs(transform.position.x);
        // Get the final converted tangent velocity
        Vector3 tangentVelocity =
            new Vector3(Mathf.Sign(transform.position.x) * Mathf.Sign(transform.position.x * transform.position.z) * playerVelocityDirection * tangentHorizontalSpeedX,
                        currentVerticalSpeed,
                        -Mathf.Sign(transform.position.z) * Mathf.Sign(transform.position.x * transform.position.z) * playerVelocityDirection * tangentHorizontalSpeedZ);

        if (test)
        {
            print("x: " + Mathf.Sign(transform.position.x) * Mathf.Sign(transform.position.x * transform.position.z) * playerVelocityDirection);
            print("z: " + -Mathf.Sign(transform.position.z) * Mathf.Sign(transform.position.x * transform.position.z) * playerVelocityDirection);
        }

        return tangentVelocity;
    }

    /// <summary>
    /// Calculate the force needed to help the player's body stays on the orbital track in side-scrolling session
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateOrbitalAdjustmentForce()
    {
        // Calculate the player's current distance from the center
        float playerCurrentDistance = new Vector3(transform.position.x, 0, transform.position.z).magnitude;
        // Calculate the coordinate of the "correct" player's position if it is on the orbit
        Vector3 targetPosition = new Vector3(targetSideScrollOrbitRadius / playerCurrentDistance * transform.position.x,
                                             0,
                                             targetSideScrollOrbitRadius / playerCurrentDistance * transform.position.z);

        if (test)
        {
            Debug.DrawRay(transform.position, targetPosition - new Vector3(transform.position.x, 0, transform.position.z), Color.red);
            print(targetPosition - new Vector3(transform.position.x, 0, transform.position.z));
        }

        return targetPosition - new Vector3(transform.position.x, 0, transform.position.z);
    }

    /// <summary>
    /// Make the player moves along the side-scrolling orbit
    /// </summary>
    public void CorrectPlayerSideScrollingOrbit()
    {
        if (test)
        {
            Debug.DrawLine(transform.position, transform.position + GetComponent<Rigidbody>().velocity, Color.white);
            Debug.DrawLine(transform.position, transform.position + CalculateTangentVelocity(), Color.blue);
            Debug.DrawLine(transform.position, transform.position + CalculateTangentVelocity() - GetComponent<Rigidbody>().velocity, Color.red);
        }

        GetComponent<Rigidbody>().AddForce(CalculateTangentVelocity() - GetComponent<Rigidbody>().velocity, ForceMode.Acceleration);
        GetComponent<Rigidbody>().AddForce(CalculateOrbitalAdjustmentForce(), ForceMode.VelocityChange);
    }
}
