using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Count the number of scientists that is immobilized by the player,
/// and drop key when player successfully attack enough scientists
/// </summary>
public class CountDeadScientistToDropKey : MonoBehaviour
{
    public int numberRequireToDropKey; // How many scientist the player needs to attack in order to get the key
    public GameObject key; // The key to be dropped

    public int deadScientistCount; // How many scientist is been attacked
    public bool keyDropped; // Has the key dropped

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Drop the key in the designated position
    /// </summary>
    /// <param name="dropPosition"></param>
    public void DropKey(Vector3 dropPosition)
    {
        keyDropped = true;
        key.transform.position = dropPosition + Vector3.up * 1.5f;
    }
}
