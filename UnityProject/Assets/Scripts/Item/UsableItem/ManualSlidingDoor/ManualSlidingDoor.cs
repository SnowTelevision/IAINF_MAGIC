using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Help the player using the manual sliding door
/// </summary>
public class ManualSlidingDoor : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetToKinematic()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void SetToNonKinematic()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
