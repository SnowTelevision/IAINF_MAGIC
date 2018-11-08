using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject guideBeam; // The guidance beam
    public LayerMask laserCollisionLayer; // What layers can the laser collide with
    public Transform laserHead; // The laser head that shoots laser
    public float handleRetractDistance; // Where the handle should retract to when the laser is on
    public Transform handleForwardRef; // The transform to get the forward direction of the handle
    public MeshRenderer handleIndicatorMesh; // The mesh that indicates the handle's status (if the handle can be used or not
    public Color handleOnColor; // The color when the handle cannot be used
    public Color handleOffColor; // The color when the handle can be used

    public bool turnedOn; // If it is turned on
    public DoorMeltByLaser hittingDoor; // The door the laser is currently hitting
    public float artificialHandleOriginalDistance; // How far the handle was away from the base when the player turn on the laser
    public Vector3 artificialHandleOriginalDirection; // What was the handle's angle on the base when the player turn on the laser

    // Use this for initialization
    void Start()
    {
        canSwitch = true;

        // Set the color to be off color
        handleIndicatorMesh.materials[1].color = handleOffColor;
        handleIndicatorMesh.materials[1].SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(handleOffColor, 0.4432004f));
    }

    // Update is called once per frame
    void Update()
    {
        if (laserBeam.activeInHierarchy)
        {
            ShootLaserBeam();
        }

        if (guideBeam.activeInHierarchy)
        {
            ShootGuideBeam();
        }
    }

    /// <summary>
    /// Switch the laser on/off
    /// </summary>
    public void TrySwitch()
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

            // If the laser is hitting a door
            if (laserHit.collider.GetComponent<DoorMeltByLaser>())
            {
                if (hittingDoor != null)
                {
                    RemoveLaserToDoor();
                }

                hittingDoor = laserHit.collider.GetComponent<DoorMeltByLaser>();
                AddLaserToDoor();
            }
            else
            {
                if (hittingDoor != null)
                {
                    RemoveLaserToDoor();
                }
            }
        }
    }

    /// <summary>
    /// Shoot the guidance beam to help the player target the laser
    /// </summary>
    public void ShootGuideBeam()
    {
        RaycastHit laserHit;
        // If the laser hit on something
        if (Physics.Raycast(laserHead.position, -laserHead.forward, out laserHit, Mathf.Infinity, laserCollisionLayer))
        {
            // Let the laser shoot at the hit position
            guideBeam.transform.LookAt(laserHit.point, Vector3.up);
            // Extend the laser to the hit position
            guideBeam.transform.localScale = Vector3.forward * laserHit.distance + Vector3.up + Vector3.left;
        }
    }

    /// <summary>
    /// Aadd a laser to a meltable door
    /// </summary>
    public void AddLaserToDoor()
    {
        if (!hittingDoor.currentHittingLasers.Contains(laserBeam))
        {
            // Add this laser to the lasers that's currently hitting this door
            hittingDoor.currentHittingLasers.Add(laserBeam);
        }
    }

    /// <summary>
    /// Remove a laser to a meltable door
    /// </summary>
    public void RemoveLaserToDoor()
    {
        // Remove this laser from the previously hitting door
        hittingDoor.currentHittingLasers.Remove(laserBeam);
        hittingDoor = null;
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
        // Deactive guide beam
        guideBeam.SetActive(false);

        // Get the handle's current distance and direction from the base
        artificialHandleOriginalDistance = GetComponentInParent<MoveLaserCutterWithArtificialHandle>().handleCurrentDistance;
        artificialHandleOriginalDirection =
            Vector3.Normalize(GetComponentInParent<MoveLaserCutterWithArtificialHandle>().transform.position - artificialLaserCutterHandle.transform.position);
        artificialHandleOriginalDirection.y = 0;
        // Turn off the handle collider
        artificialLaserCutterHandle.GetComponent<SphereCollider>().enabled = false;
        // Prevent the player from moving the laser cutter when it is turned on.
        artificialLaserCutterHandle.enabled = false;

        // Retract the handle
        for (float t = 0; t < 1; t += Time.deltaTime / animationTime)
        {
            artificialLaserCutterHandle.transform.position +=
                artificialHandleOriginalDirection * (artificialHandleOriginalDistance - handleRetractDistance) * Time.deltaTime / animationTime;

            // Change the handle indicator color
            handleIndicatorMesh.materials[1].color = Color.Lerp(handleOnColor, handleOffColor, t);
            handleIndicatorMesh.materials[1].SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(Color.Lerp(handleOnColor, handleOffColor, t), 0.4432004f));

            // Make sure the item status indicator is the "Is Controlling" color
            GetComponent<ItemInfo>().ChangeIndicatorColor(GetComponent<ItemInfo>().isControllingStatusColor, 1);

            yield return null;
        }

        //yield return new WaitForSeconds(animationTime);

        turnedOn = true;

        yield return new WaitForEndOfFrame();
        // Active laser beam
        laserBeam.SetActive(true);
        // Enable the player to switch
        canSwitch = true;

        // Change the item status indicator to the "Default Status" color
        GetComponent<ItemInfo>().ChangeIndicatorColor(GetComponent<ItemInfo>().defaultStatusColor, 1);
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
        // Deactive laser beam
        laserBeam.SetActive(false);
        yield return new WaitForEndOfFrame();
        turnedOn = false;

        // Extend the handle
        for (float t = 0; t < 1; t += Time.deltaTime / animationTime)
        {
            artificialLaserCutterHandle.transform.position -=
                artificialHandleOriginalDirection * (artificialHandleOriginalDistance - handleRetractDistance) * Time.deltaTime / animationTime;

            // Change the handle indicator color
            handleIndicatorMesh.materials[1].color = Color.Lerp(handleOffColor, handleOnColor, t);
            handleIndicatorMesh.materials[1].SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(Color.Lerp(handleOffColor, handleOnColor, t), 0.4432004f));

            // Make sure the item status indicator is the "Is Controlling" color
            GetComponent<ItemInfo>().ChangeIndicatorColor(GetComponent<ItemInfo>().isControllingStatusColor, 1);

            yield return null;
        }
        // yield return new WaitForSeconds(animationTime);

        // Active guide beam
        guideBeam.SetActive(true);
        // Enable the player to move the laser cutter when it is turned off.
        artificialLaserCutterHandle.enabled = true;
        // Turn on the handle collider
        artificialLaserCutterHandle.GetComponent<SphereCollider>().enabled = true;
        // Enable the player to switch
        canSwitch = true;

        // Change the item status indicator to the "Default Status" color
        GetComponent<ItemInfo>().ChangeIndicatorColor(GetComponent<ItemInfo>().defaultStatusColor, 1);
    }
}
