using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerMovement : TutorialEvent
{

    // Use this for initialization
    void Start()
    {

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerExit(Collider other)
    {
        // If it is the player's body
        if (other.GetComponent<PlayerInfo>())
        {
            eventFinished = true;
        }
    }
}
