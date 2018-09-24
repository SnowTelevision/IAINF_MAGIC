using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This key can be stole by the player. What will happen when the player steal it for the first time
/// </summary>
public class StealableKey : MonoBehaviour
{


    public bool stole; // Has this key been stolen by the player

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// When the player begin to hold the key
    /// </summary>
    public void OnPlayerHoldKey()
    {
        if (!stole)
        {
            stole = true;
        }

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.parent = null;
    }
}
