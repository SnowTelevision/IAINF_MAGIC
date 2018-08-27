using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Let the arm use items
/// </summary>
public class ArmUseItem : MonoBehaviour
{


    public GameObject currentlyHoldingItem; // The item that is currently holding by this arm
    public bool hasTriggerReleased; // Has the trigger being released since the last pushed down
    public UnityEvent useItem; // The event to be triggered for the holding item when the player start using it
    public UnityEvent stopUsingItem; // The event to be triggered for the holding item when the player stop using it
    public delegate void UseItemDelegateClass();
    public delegate void StopUsingItemDelegateClass();
    public UseItemDelegateClass useItemDelegate;
    public StopUsingItemDelegateClass stopUsingItemDelegate;

    // Use this for initialization
    void Start()
    {
        //hasTriggerReleased = true;
    }

    // Update is called once per frame
    void Update()
    {
        DetectIfUseItem();
    }

    /// <summary>
    /// Detect if the armTip is using an item (need to implement hold down to use continuously)
    /// </summary>
    public void DetectIfUseItem()
    {
        // If the arm tip is holding an usable item
        if (currentlyHoldingItem != null && currentlyHoldingItem.GetComponent<ItemInfo>().canUse)
        {
            if (GetComponentInParent<ControlArm>().isLeftArm)
            {
                // If the left trigger is pressed down
                if (Input.GetAxis("LeftTrigger") >= GetComponentInParent<ControlArm>().triggerThreshold)
                {
                    // If the player just pressed it down
                    if (hasTriggerReleased)
                    {
                        hasTriggerReleased = false;
                        StartUsingItem();
                    }
                }
                else
                {
                    hasTriggerReleased = true;
                    StopUsingItem();
                }
            }

            if (!GetComponentInParent<ControlArm>().isLeftArm)
            {
                // If the right armTip is not holding an item and the right trigger is pressed down
                if (Input.GetAxis("RightTrigger") >= GetComponentInParent<ControlArm>().triggerThreshold)
                {
                    // If the player just pressed it down
                    if (hasTriggerReleased)
                    {
                        hasTriggerReleased = false;
                        StartUsingItem();
                    }
                }
                else
                {
                    hasTriggerReleased = true;
                    StopUsingItem();
                }
            }
        }
    }

    /// <summary>
    /// Invoke the start using event that is currently registered to this armTip
    /// </summary>
    public void StartUsingItem()
    {
        useItem.Invoke();
        useItemDelegate();
    }

    /// <summary>
    /// Invoke the stop using event that is currently registered to this armTip
    /// </summary>
    public void StopUsingItem()
    {
        stopUsingItem.Invoke();
        stopUsingItemDelegate();
    }
}
