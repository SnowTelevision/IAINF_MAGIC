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

    public bool inWater; // Is the body in the water
    public static Transform sGameCamera; // The static reference to gameCamera
    public static bool isSideScroller; // If the game is in side-scroller mode

    // Test
    public bool test;

    private void Awake()
    {
        // Assigning static references for variebles that has to be assigned in the editor thus cannot be static.
        sGameCamera = gameCamera;
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
}
