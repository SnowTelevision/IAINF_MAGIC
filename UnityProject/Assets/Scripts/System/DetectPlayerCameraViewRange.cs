using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detect whether this object can be seen by the player's camera
/// </summary>
public class DetectPlayerCameraViewRange : MonoBehaviour
{


    public bool isInCamera; // Is this object in the player's camera

    // Use this for initialization
    void Start()
    {
        // Adds a mesh renderer to the detector
        // gameObject.AddComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnBecameVisible()
    {
        //print("In camera");
        isInCamera = true;
    }

    private void OnBecameInvisible()
    {
        //print("Out camera");
        isInCamera = false;
    }
}
