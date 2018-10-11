using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detect if the player picks up the key
/// </summary>
public class PlayerPickedUpKey : TutorialEvent
{
    public ItemInfo key; // The key the player has to pick up

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        // If the player picks up the key
        if (key.isBeingHeld)
        {
            eventFinished = true;
        }

        base.Update();
    }


}
