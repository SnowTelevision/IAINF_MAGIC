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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Make the laser base rotate with the handle
        laserCutterBase.LookAt(artificialHandle);
    }
}
