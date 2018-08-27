using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the camera to look at the security
/// </summary>
public class CameraLookAtSecurityEvent : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Re-enable the camera following
        if (GetComponent<LinearObjectMovement>().animationFinished)
        {
            GetComponent<FollowCamera>().enabled = true;
            GameManager.ScriptedEventStop();
            this.enabled = false;
        }
    }

    /// <summary>
    /// Start move the camera towards the security
    /// </summary>
    public void StartEvent()
    {
        StartCoroutine(GetComponent<LinearObjectMovement>().Animate());
        GetComponent<FollowCamera>().enabled = false;
    }
}
