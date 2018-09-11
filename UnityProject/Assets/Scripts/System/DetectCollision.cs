using UnityEngine;

/// <summary>
/// Detects collision
/// </summary>
public class DetectCollision : MonoBehaviour
{
    public bool ignoreTrigger; // If ignore trigger

    public bool isColliding; // If the arm is colliding something
    public Vector3 collidingPoint; // The colliding position
    public GameObject collidingObject; // The object it is colliding with
    public Collision currentCollision; // The collision

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

        currentCollision = collision;
        collidingObject = collision.gameObject;
    }

    private void OnCollisionStay(Collision collision)
    {
        VerifyCollision(false, collision.gameObject);

        isColliding = true;
        collidingObject = collision.gameObject;
        collidingPoint = collision.contacts[0].point;
    }

    private void OnCollisionExit(Collision collision)
    {
        VerifyCollision(false, collision.gameObject);

        currentCollision = null;
        collidingObject = null;
        isColliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ignoreTrigger)
        {
            return;
        }

        VerifyCollision(false, other.gameObject);

        collidingObject = other.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (ignoreTrigger)
        {
            return;
        }

        VerifyCollision(false, other.gameObject);

        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (ignoreTrigger)
        {
            return;
        }

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
