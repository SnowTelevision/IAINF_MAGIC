using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the laser cutter's height and orientation with an invisible, artificial "handle"
/// </summary>
public class MoveLaserCutterWithArtificialHandle : MonoBehaviour
{
    public Transform artificialHandle; // The artificial handle which will be grabbed by the player
    public Transform laserCutterBase; // The base of the laser cutter
    public Transform laserCutterHead; // The laser head that shoots laser
    public Transform artificialHandleModel; // The model of the laser handle
    public float maxLaserArmTilt; // How much angle degree the laser arm can raise up
    public float handleDistanceDeadzone; // How much deadzone of the handle's min and max distance has 
                                         //(So that the player doesn't need to pull the handle all the way out to make the laser arm flat)
    public float handleMinDistance; // How close the handle can be to the base
    public float handleMaxDistance; // How far the handle can be to the base
    public float armFlatTiltAngleOffset; // How much the arm should tilt up from the horizontal plane when it is flat
                                         // Make sure this angle is smaller than the maxLaserArmTile angle
    public GameObject laserRoomCenter; // The object represents the center of the room
    public float laserRoomCameraHeight; // How high the camera should be when it zoom out on the entire room

    public float handleCurrentDistance; // How far is the handle currently away from the base
    public Transform laserArm; // The laser arm that the laser gun is mounted on
    public Transform laserGun; // The laser gun that shoots the laser
    public Coroutine delayedCameraChangeCoroutine; // The coroutine that wait for player move laser handle then change the camera

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Only update the laser arm's transform when it is turned off
        if (artificialHandle.GetComponent<ItemInfo>().enabled)
        {
            UpdateBaseRotation();
            UpdateHandleRotation();
        }

        // Get the handle's current distance from the base
        handleCurrentDistance = Vector3.Distance(transform.position, artificialHandle.position);

        // Only update the laser arm's transform when it is turned off
        if (artificialHandle.GetComponent<ItemInfo>().enabled)
        {
            UpdateArmTilt();
        }
    }

    /// <summary>
    /// Make the laser base rotate with the handle
    /// </summary>
    public void UpdateBaseRotation()
    {
        laserCutterBase.LookAt(artificialHandle, Vector3.up);
        laserCutterBase.localEulerAngles =
            new Vector3(0, laserCutterBase.localEulerAngles.y, 0);
    }

    /// <summary>
    /// Make the handle rotate with the base
    /// </summary>
    public void UpdateHandleRotation()
    {
        artificialHandleModel.LookAt(laserCutterBase, Vector3.up);
        artificialHandleModel.localEulerAngles =
            new Vector3(0, artificialHandleModel.localEulerAngles.y, 0);
    }

    /// <summary>
    /// Make pushing and pulling handle control the laser arm's tilt
    /// </summary>
    public void UpdateArmTilt()
    {
        // Clamp the handle's distance within the deadzone
        float clampedHandleDistance = Mathf.Clamp(handleCurrentDistance, handleMinDistance + handleDistanceDeadzone, handleMaxDistance - handleDistanceDeadzone);
        // Normalize the handle's current distance
        float normalizedHandleDistance = (clampedHandleDistance - (handleMinDistance + handleDistanceDeadzone)) /
                                         (handleMaxDistance - handleDistanceDeadzone - (handleMinDistance + handleDistanceDeadzone));
        // Get the tilt angle for the laser arm
        float tiltAngle = (maxLaserArmTilt - armFlatTiltAngleOffset) * (1 - normalizedHandleDistance);
        // Rotate the laser arm and the laser gun
        laserArm.localEulerAngles = Vector3.right * (tiltAngle + armFlatTiltAngleOffset);
        laserGun.localEulerAngles = -Vector3.right * tiltAngle * 2;
    }

    /// <summary>
    /// Make the camera look at the entire laser room
    /// </summary>
    public void CameraLookRoom()
    {
        // Stop existing change camera coroutine
        if (delayedCameraChangeCoroutine != null)
        {
            StopCoroutine(delayedCameraChangeCoroutine);
        }

        delayedCameraChangeCoroutine = StartCoroutine(CameraWaitForPlayerAction(artificialHandle.position));
    }

    /// <summary>
    /// Let the camera wait until the player start to move the handle then change the camera to look entire room
    /// </summary>
    /// <param name="handleStartPosition"></param>
    /// <returns></returns>
    public IEnumerator CameraWaitForPlayerAction(Vector3 handleStartPosition)
    {
        while (Vector3.Distance(artificialHandle.position, handleStartPosition) < 0.1f)
        {
            yield return null;
        }

        FollowCamera.CameraChangeFollowingTarget(laserRoomCenter, laserRoomCameraHeight);
    }

    /// <summary>
    /// Make the camera follows player
    /// </summary>
    public void CameraLookPlayer()
    {
        // Stop existing change camera coroutine
        if (delayedCameraChangeCoroutine != null)
        {
            StopCoroutine(delayedCameraChangeCoroutine);
            delayedCameraChangeCoroutine = null;
        }

        FollowCamera.CameraChangeFollowingTarget(GameManager.sPlayer, FollowCamera.mainGameCamera.defaultCameraHeight);
    }
}
