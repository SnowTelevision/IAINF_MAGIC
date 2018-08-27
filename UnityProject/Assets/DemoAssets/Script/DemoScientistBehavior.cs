using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the dumb scientist in the magic tutorial demo
/// </summary>
public class DemoScientistBehavior : MonoBehaviour
{
    public GameObject key; // The door key
    public GameObject tutorialPickUp; // The tutorial that teaches pick up item

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetComponent<LinearObjectMovement>().Animate());
        GetComponentInChildren<Actions>().Walk();
    }

    // Update is called once per frame
    void Update()
    {
        // Drop the key if the scientist enters the room
        if (transform.position.z < 10.1f &&
            !GetComponent<LinearObjectMovement>().isAnimationRunning &&
            key.transform.parent != null)
        {
            GetComponentInChildren<Actions>().Stay();
            key.GetComponent<ItemInfo>().isBeingHeld = false;
            key.GetComponent<Rigidbody>().isKinematic = false;
            key.transform.parent = null;
            transform.LookAt(GameManager.sPlayer.transform, Vector3.up);
        }

        // Turn back and run away
        if (transform.position.z > 10 &&
            key.transform.parent == null &&
            GetComponent<LinearObjectMovement>().isAnimationRunning)
        {
            transform.eulerAngles = Vector3.zero;
            GetComponentInChildren<Actions>().Run();
            Destroy(gameObject, 8);
            this.enabled = false;
        }
    }

    private void OnBecameVisible()
    {
        //print("In camera");
        GameManager.ScriptedEventStart();
    }

    private void OnBecameInvisible()
    {
        //print("Out camera");
        GameManager.ScriptedEventStop();
        tutorialPickUp.SetActive(true);
    }
}
