using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmBehavior : MonoBehaviour
{
    public float popOutImpulse; // The force applied on it when it pops out
    public float popOutAngle; // The initial angle when it pops out
    public float minimumBreakSpeed; // How fast the player has to throw the alarm to make it break and attract enemies
    public GameObject armDetector; // The trigger that detects player's armTips
    public GameObject alarmBase; // The base that the alarm is attached to

    public bool isPopOut; // Is the alarm poped out
    public float playerThrowSpeed; // How fast did the player throw the alarm

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the player knock the alarm with the wrench
        if (!isPopOut && collision.collider.name == "Wrench")
        {
            isPopOut = true;

            // Turn off the collider on the wrench for 0.2s
            StartCoroutine(DelayedTurnOnCollision(collision.collider));
            collision.collider.enabled = false;
            // Turn off the collider on the alarm base for 0.2s
            StartCoroutine(DelayedTurnOnCollision(alarmBase.GetComponent<Collider>()));
            alarmBase.GetComponent<Collider>().enabled = false;

            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<ItemInfo>().enabled = true;
            GetComponent<Rigidbody>().AddForce(popOutImpulse * (transform.forward + Vector3.up * Mathf.Tan(popOutAngle * Mathf.Deg2Rad)),
                                               ForceMode.Impulse);
            //armDetector.SetActive(true);
        }

        // If the alarm hit something with an initial throw speed greater than the break threshold
        if (isPopOut && playerThrowSpeed >= minimumBreakSpeed)
        {
            AlarmBreak();
        }
    }

    public IEnumerator DelayedTurnOnCollision(Collider colliderToBeTurnedOn)
    {
        yield return new WaitForSeconds(0.2f);
        colliderToBeTurnedOn.enabled = true;
    }

    /// <summary>
    /// Give the alarm some upward force when the player throws the alarm
    /// </summary>
    public void ThrowUp()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * Mathf.Clamp(playerThrowSpeed * 0.03f, 0, 0.5f), ForceMode.Impulse);
    }

    /// <summary>
    /// Gets the initial speed when the player throw it out
    /// </summary>
    public void GetThrowSpeed()
    {
        playerThrowSpeed = GetComponent<Rigidbody>().velocity.magnitude;
    }

    /// <summary>
    /// When the alarm is threw by the player above a certain speed and collide on something and breaks
    /// </summary>
    public void AlarmBreak()
    {
        print("break");
        //Destroy(gameObject);
    }
}
