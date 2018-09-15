using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turns on and off the laser cutter
/// </summary>
public class LaserCutterSwitch : MonoBehaviour
{
    public bool canSwitch; // Can the player now switch on or off the laser cutter
    public float animationTime; // The animation time of the laser cutter
    public ItemInfo artificialLaserCutterHandle; // The artificial handle for the player to move the laser cutter
    public GameObject laserBeam; // The laser beam
    public LayerMask laserCollisionLayer; // What layers can the laser collide with
    public Transform laserHead; // The laser head that shoots laser

    public bool turnedOn; // If it is turned on

    // Use this for initialization
    void Start()
    {
        canSwitch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnedOn)
        {
            ShootLaserBeam();
        }
    }

    /// <summary>
    /// Switch the laser on/off
    /// </summary>
    public void Switch()
    {
        // If the laser cutter can be switch on / off
        if (canSwitch)
        {
            if (turnedOn)
            {
                StartTurningOff();
            }
            else
            {
                StartTurningOn();
            }
        }
    }

    /// <summary>
    /// Shoots laser beam in the laser gun's direction
    /// </summary>
    public void ShootLaserBeam()
    {
        RaycastHit laserHit;
        // If the laser hit on something
        if (Physics.Raycast(laserHead.position, -laserHead.forward, out laserHit, Mathf.Infinity, laserCollisionLayer))
        {
            // Let the laser shoot at the hit position
            laserBeam.transform.LookAt(laserHit.point, Vector3.up);
            // Extend the laser to the hit position
            laserBeam.transform.localScale = Vector3.forward * laserHit.distance + Vector3.up + Vector3.left;
        }
    }

    /// <summary>
    /// Start the turning on process
    /// </summary>
    public void StartTurningOn()
    {
        StartCoroutine(TurningOnProcess());
    }

    /// <summary>
    /// The process to turn on the laser cutter
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurningOnProcess()
    {
        // Prevent the player from switch again during a switch animation
        canSwitch = false;
        // Prevent the player from moving the laser cutter when it is turned on.
        artificialLaserCutterHandle.enabled = false;
        yield return new WaitForSeconds(animationTime);

        turnedOn = true;

        yield return new WaitForEndOfFrame();
        // Active laser beam
        laserBeam.SetActive(true);
        // Enable the player to switch
        canSwitch = true;
    }

    /// <summary>
    /// Start the turning off process
    /// </summary>
    public void StartTurningOff()
    {
        StartCoroutine(TurningOffProcess());
    }

    /// <summary>
    /// The process to turn off the laser cutter
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurningOffProcess()
    {
        // Prevent the player from switch again during a switch animation
        canSwitch = false;
        yield return new WaitForSeconds(animationTime);

        // Deactive laser beam
        laserBeam.SetActive(false);
        yield return new WaitForEndOfFrame();
        turnedOn = false;
        // Enable the player to move the laser cutter when it is turned off.
        artificialLaserCutterHandle.enabled = true;
        // Enable the player to switch
        canSwitch = true;
    }
}
