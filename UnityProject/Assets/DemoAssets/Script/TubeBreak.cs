using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make the tube break after some hit
/// </summary>
public class TubeBreak : MonoBehaviour
{
    public int hitToBreak; // The number of hit to break the tube
    public GameObject tutorialTextTriggerBeforeBreak; // The tutorial text to show before the tube breaks
    public GameObject tutorialTextTriggerAfterBreak; // The tutorial text to show after the tube breaks

    public int timeWasHit; // How many hit has the tube received
    public GameObject lastCollidedArmTip; // The last armTip that hit the tube

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeWasHit >= hitToBreak)
        {
            Destroy(tutorialTextTriggerBeforeBreak);
            tutorialTextTriggerAfterBreak.SetActive(true);
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Count a hit if it is hit by a player's armTip
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerArm") &&
            collision.collider.gameObject != lastCollidedArmTip)
        {
            lastCollidedArmTip = collision.collider.gameObject;
            timeWasHit++;
        }
    }
}
