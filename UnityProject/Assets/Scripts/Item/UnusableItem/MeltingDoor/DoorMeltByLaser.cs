using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Let the door melt when the required number of lasers are shooting at it
/// </summary>
public class DoorMeltByLaser : MonoBehaviour
{
    public int lasersNeedToMelt; // How many laser beams is required to melt this door

    public List<GameObject> currentHittingLasers; // The lasers that are currently shooting at it

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHittingLasers.Count >= lasersNeedToMelt)
        {
            gameObject.SetActive(false);
            this.enabled = false;
        }
    }
}
