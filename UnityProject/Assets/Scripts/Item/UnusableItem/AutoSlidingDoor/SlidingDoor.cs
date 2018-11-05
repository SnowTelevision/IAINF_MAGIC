using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a sliding door
/// All door should have its local forward pointing into the room
/// </summary>
public class SlidingDoor : MonoBehaviour
{
    public bool requireKey; // Does this door needs an item that work as a key to open it
    public int keyCode; // The code of the key to check if the key matches this door
    public bool requirePass; // Does this door needs the player to put in a passcode to open it
    public int passcode; // The passcode this door is check againest when the player enters it
    public bool openLeft; // Does this door slides towards the negative(left) local x direction or the positive(right) local x direction
    public float doorKeepOpenDuration; // How long the door will stay open after it is opened by a key or pass code
    public bool detectNPC; // Can this door detect close-by NPC and open

    public Coroutine controlDoorAnimationCoroutine; // The coroutine that runs the door's open&close animation
    public float lastKeyPassEnterTime; // The latest time a correct key or pass code is used on this door
    //public List<KeyInfo> touchingKeys; // The keys that are currently touching the door
    public int closeByBodyCount; // How many bodies (player or NPC) are close to the door

    // Use this for initialization
    void Start()
    {
        //// Rotate the door if the door is open to the right
        //if (!openLeft)
        //{
        //    transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (KeyInfo k in touchingKeys)
        //{
        //    // Remove the key from the touching key list if the key is no longer holding by a character
        //    if (!k.GetComponent<ItemInfo>().isBeingHeld)
        //    {
        //        touchingKeys.Remove(k);
        //        // If the key is the last key that's being dropped then close the door
        //        if(touchingKeys.Count == 0)
        //        {
        //            controlDoorAnimationCoroutine = StartCoroutine(GetComponentsInChildren<LinearObjectMovement>()[1].Animate());
        //        }
        //    }
        //}

        // If the door requires key or pass code to open
        if (requireKey || requirePass)
        {
            // If a body is not close to the door and the duration of the door should keep open passed then close the door
            if (Time.time - lastKeyPassEnterTime >= doorKeepOpenDuration && closeByBodyCount == 0)
            {
                controlDoorAnimationCoroutine = StartCoroutine(GetComponentsInChildren<LinearObjectMovement>()[1].Animate());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool openDoor = false;

        // If the player is close to the door
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
        {
            closeByBodyCount++;
        }
        // If this door can detect NPC
        if (detectNPC && other.tag == "MovingEnemy")
        {
            closeByBodyCount++;
        }

        if (requireKey)
        {
            //// If the item is a key and is currently holding by the player or some NPC
            //if (other.GetComponent<KeyInfo>() && other.GetComponent<ItemInfo>().isBeingHeld)
            //{
            //    if (other.GetComponent<KeyInfo>().keyCode == keyCode)
            //    {
            //        openDoor = DetectIfUseKey();
            //        touchingKeys.Add(other.GetComponent<KeyInfo>());
            //    }
            //}
            // If the item is a key and is currently holding by the player or some NPC
            if (other.GetComponent<KeyInfo>() && other.GetComponent<ItemInfo>().isBeingHeld)
            {
                // If the key code match with the door
                if (other.GetComponent<KeyInfo>().keyCode == keyCode)
                {
                    openDoor = true;
                }
            }
        }
        else if (requirePass)
        {

        }
        // If the door don't require any item or code
        else
        {
            // If the player is close to the door then open it
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
            {
                openDoor = true;
            }
            // If this door can detect NPC
            if (detectNPC && other.tag == "MovingEnemy")
            {
                openDoor = true;
            }
        }

        if (openDoor)
        {
            OpenDoor();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //// If this door needs a key to open and a key touching it is just picked up by someone
        //if (requireKey && other.GetComponent<KeyInfo>() &&
        //    other.GetComponent<KeyInfo>().keyCode == keyCode &&
        //    !touchingKeys.Contains(other.GetComponent<KeyInfo>()) &&
        //    other.GetComponent<ItemInfo>().isBeingHeld)
        //{
        //    // If the key is the only key touching it
        //    if (DetectIfUseKey())
        //    {
        //        controlDoorAnimationCoroutine = StartCoroutine(GetComponentsInChildren<LinearObjectMovement>()[0].Animate());
        //    }
        //    // Add the key back to the touching key list
        //    touchingKeys.Add(other.GetComponent<KeyInfo>());
        //}
        // If this door needs a key to open and a key touching it is just picked up by someone and the door is closed
        if (requireKey &&
            Time.time - lastKeyPassEnterTime >= doorKeepOpenDuration &&
            other.GetComponent<KeyInfo>() &&
            other.GetComponent<KeyInfo>().keyCode == keyCode &&
            other.GetComponent<ItemInfo>().isBeingHeld)
        {
            //bool openDoor = true;

            //// If the door is currently opening and closing
            //foreach(LinearObjectMovement l in GetComponentsInChildren<LinearObjectMovement>())
            //{
            //    if (l.isAnimationRunning)
            //    {
            //        openDoor = false;
            //    }
            //}

            //if (openDoor)
            {
                OpenDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool closeDoor = false;

        // If the player is close to the door
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
        {
            closeByBodyCount--;
        }
        // If this door can detect NPC
        if (detectNPC && other.tag == "MovingEnemy")
        {
            closeByBodyCount--;
        }

        if (requireKey)
        {
            //// If the item is a key and is currently holding by the player or some NPC
            //if (other.GetComponent<KeyInfo>() && other.GetComponent<ItemInfo>().isBeingHeld)
            //{
            //    // If the key code match with the door
            //    if (other.GetComponent<KeyInfo>().keyCode == keyCode)
            //    {
            //        // Remove the key from the touching key list
            //        touchingKeys.Remove(other.GetComponent<KeyInfo>());
            //        closeDoor = DetectIfUseKey();
            //    }
            //}
        }
        else if (requirePass)
        {

        }
        // If the door don't require any item or code
        else
        {
            // If the player is leaving the door then close it
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
            {
                closeDoor = true;
            }
            // If this door can detect NPC
            if (detectNPC && other.tag == "MovingEnemy")
            {
                closeDoor = true;
            }
        }

        if (closeDoor && closeByBodyCount == 0)
        {
            CloseDoor();
        }
    }

    /// <summary>
    /// Open the door
    /// </summary>
    public void OpenDoor()
    {
        if (controlDoorAnimationCoroutine != null)
        {
            StopCoroutine(controlDoorAnimationCoroutine);
        }

        lastKeyPassEnterTime = Time.time;
        controlDoorAnimationCoroutine = StartCoroutine(GetComponentsInChildren<LinearObjectMovement>()[0].Animate());
    }

    /// <summary>
    /// Close the door
    /// </summary>
    public void CloseDoor()
    {
        if (controlDoorAnimationCoroutine != null)
        {
            StopCoroutine(controlDoorAnimationCoroutine);
        }

        controlDoorAnimationCoroutine = StartCoroutine(GetComponentsInChildren<LinearObjectMovement>()[1].Animate());
    }

    /// <summary>
    /// Completely shut off the door, prevent the player from going back
    /// </summary>
    public void SealDoor()
    {
        // Disable the open door trigger
        GetComponent<BoxCollider>().enabled = false;

        if (controlDoorAnimationCoroutine != null)
        {
            StopCoroutine(controlDoorAnimationCoroutine);
        }

        GetComponentsInChildren<LinearObjectMovement>()[1].StartAnimation();
        this.enabled = false;
    }

    ///// <summary>
    ///// When a key start/stop touching the door, detect if anything happenes to the door
    ///// </summary>
    ///// <returns></returns>
    //public bool DetectIfUseKey()
    //{
    //    if (touchingKeys.Count == 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
