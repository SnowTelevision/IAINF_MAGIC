using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTutorialAnimation : MonoBehaviour
{
    public GameObject controlledAnimation; // The tutorial animation this script is controlling

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Display the tutorial animation UI
    /// </summary>
    public void DisplayAnimation()
    {
        controlledAnimation.SetActive(true);
    }

    /// <summary>
    /// Hide the tutorial animation UI
    /// </summary>
    public void HideAnimation()
    {
        controlledAnimation.SetActive(false);
    }

    private void OnDestroy()
    {
        // Stop the animation when it is destroyed because the player finished the tutorial objective
        HideAnimation();
    }
}
