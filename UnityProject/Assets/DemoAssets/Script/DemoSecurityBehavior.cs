using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the dumb security in the magic tutorial demo
/// </summary>
public class DemoSecurityBehavior : MonoBehaviour
{
    public GameObject pistol; // The pistol held by the security
    public GameObject tutorialPickUpAndUse; // The tutorial that teaches pick up item and use it
    public GameObject normalPistolBullet; // The normal pistol bullet that won't send the player back

    public Quaternion initialRotation; // The initial rotation of the security

    // Use this for initialization
    void Start()
    {
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Look at the player if it detects the player
        if (pistol.GetComponent<ItemInfo>().usingItem != null)
        {
            transform.LookAt(GameManager.sPlayer.transform, Vector3.up);
        }
        else
        {
            transform.rotation = initialRotation;
        }
    }

    ///// <summary>
    ///// Start to shoot pistol at player
    ///// </summary>
    //public void StartShootPlayer()
    //{

    //}

    private void OnCollisionEnter(Collision collision)
    {
        // Prevent it shot by its own bullet
        if (collision.collider.GetComponent<SimplePistolBullet>() &&
            collision.collider.GetComponent<SimplePistolBullet>().owner == pistol.GetComponent<SimplePistol>())
        {
            return;
        }

        // If it is hit
        if (collision.collider.name == "SimplePistolBulletWarp(Clone)" ||
            collision.collider.name == "BatWrap")
        {
            SecurityDie();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(this.name);

        // Prevent it shot by its own bullet
        if (other.GetComponent<SimplePistolBullet>() &&
            other.GetComponent<SimplePistolBullet>().owner == pistol.GetComponent<SimplePistol>())
        {
            return;
        }

        // If it is hit
        if (other.name == "SimplePistolBulletWarp(Clone)" ||
            other.name == "CrowbarWrap")
        {
            SecurityDie();
        }
    }

    /// <summary>
    /// What happens when the security dead
    /// </summary>
    public void SecurityDie()
    {
        if (tutorialPickUpAndUse != null)
        {
            tutorialPickUpAndUse.SetActive(true);
            foreach(SimplePistol p in FindObjectsOfType<SimplePistol>())
            {
                p.pistolBullet = normalPistolBullet;
            }
        }

        pistol.GetComponent<ItemInfo>().eventCoolDown = Mathf.Infinity;
        pistol.GetComponent<Rigidbody>().isKinematic = false;
        pistol.transform.parent = null;
        Destroy(gameObject);
    }
}
