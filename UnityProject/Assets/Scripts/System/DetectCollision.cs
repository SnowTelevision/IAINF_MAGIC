using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects collision
/// </summary>
public class DetectCollision : MonoBehaviour
{


    public bool isColliding; // If the arm is colliding something
    public Vector3 collidingPoint; // The colliding position
    public GameObject collidingObject; // The object it is colliding with

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        isColliding = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        VerifyCollision(false, collision.gameObject);

        collidingObject = collision.gameObject;
    }

    private void OnCollisionStay(Collision collision)
    {
        VerifyCollision(false, collision.gameObject);

        isColliding = true;
        collidingPoint = collision.contacts[0].point;
    }

    private void OnCollisionExit(Collision collision)
    {
        VerifyCollision(false, collision.gameObject);

        collidingObject = null;
        isColliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        VerifyCollision(false, other.gameObject);

        collidingObject = other.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        VerifyCollision(false, other.gameObject);

        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        VerifyCollision(false, other.gameObject);

        collidingObject = null;
        isColliding = false;
    }

    /// <summary>
    /// Determine if the collider or trigger should be detected
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="other"></param>
    public void VerifyCollision(bool trigger, GameObject other)
    {
        // Don't detect tutorial trigger box
        if (other.GetComponent<TriggerDetectStartEvent>())
        {
            return;
        }

        // If this is the armTip and the collider is not an item
        if (trigger && GetComponent<ArmUseItem>() && !other.GetComponent<ItemInfo>())
        {
            return;
        }
    }
}
