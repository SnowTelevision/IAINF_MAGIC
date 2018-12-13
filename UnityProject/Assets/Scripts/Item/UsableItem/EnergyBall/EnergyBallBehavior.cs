using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallBehavior : MonoBehaviour
{
    public GameObject energyBallExplosionEffect; // The visual effect when the energy ball explode

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
            GetComponent<Rigidbody>().isKinematic = true;
            EnergyBallCollideExplode();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Energy ball explode if it collides after the player dropped it
        if (followingArmTip.GetComponent<ArmUseItem>().currentlyHoldingItem == null)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            EnergyBallCollideExplode();
        }
    }

    /// <summary>
    /// When the energy ball collide on something
    /// </summary>
    public void EnergyBallCollideExplode()
    {
        energyBallExplosionEffect.transform.parent = null;
        energyBallExplosionEffect.SetActive(true);

        Destroy(gameObject);
        Destroy(energyBallExplosionEffect, 5f);
    }

    // Activate the energy ball when the player clicks the trigger
    public void ActivateEnergyBall()
    {
        activated = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;

        // Stop showing energy ball self dialog
        if (ChangeSelfDialogImage.showImageCoroutine != null)
        {
            StopCoroutine(ChangeSelfDialogImage.showImageCoroutine);
        }
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
