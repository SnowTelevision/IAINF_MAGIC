using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallBehavior : MonoBehaviour
{


    public Transform followingArmTip; // The armTip this energy ball should follow
    public bool activated; // Is the energy ball activated? (Has the player click the trigger to hold it)
    public float playerThrowSpeed; // How fast did the player throw the energy ball

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Let the energy ball follows the armTip's position
        if (!activated)
        {
            transform.position = followingArmTip.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Energy ball explode if it collides after the player dropped it
        if (followingArmTip.GetComponent<ArmUseItem>().currentlyHoldingItem == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Energy ball explode if it collides after the player dropped it
        if (followingArmTip.GetComponent<ArmUseItem>().currentlyHoldingItem == null)
        {
            Destroy(gameObject);
        }
    }

    // Activate the energy ball when the player clicks the trigger
    public void ActivateEnergyBall()
    {
        activated = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    /// <summary>
    /// Give the energy ball some upward force when the player throws the alarm
    /// </summary>
    public void ThrowUp()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * Mathf.Clamp(playerThrowSpeed * 0.02f, 0, 0.4f), ForceMode.Impulse);
    }

    /// <summary>
    /// Gets the initial speed when the player throw it out
    /// </summary>
    public void GetThrowSpeed()
    {
        playerThrowSpeed = GetComponent<Rigidbody>().velocity.magnitude;
    }
}
