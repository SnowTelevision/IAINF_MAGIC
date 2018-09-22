using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistDie : MonoBehaviour
{
    public GameObject carryingKey; // The key this scientist is carrying
    public int keyCode; // The code for this scientist's key

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// What happenes when an scientist die
    /// </summary>
    public void OnEngineerDie()
    {
        // Create a new dropped key
        GameObject droppedKey = Instantiate(carryingKey, transform.position - transform.forward - Vector3.up, transform.rotation);
        // Assign corresponding keycode
        droppedKey.GetComponent<KeyInfo>().keyCode = keyCode;

        Destroy(this);
    }
}
