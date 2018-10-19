using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemInfo : MonoBehaviour
{
    public bool canUse; // Is this an usable item? If it is not then the player has to keep holding down the grab item button to hold it
    public float itemWeight; // The item's weight. If it is smaller or equal than how much the arm can carry, it should be able to be lifted by the arm and not touching the ground
    public UnityEvent setupEvent; // An event to be triggered when the item is picked up
    public UnityEvent singleUseEvent; // An event to be triggered for a single use of the item
    public UnityEvent stopUsingEvent; // An event to be triggered when the item is stop being used
    public UnityEvent resetEvent; // An event to be triggered when the item is dropped
    public float eventCoolDown; // When the player is holding down the use button and constantly using the item, how long it should wait before each use
    public int useLimit; // How many times can this item being used (0 means infinite)
    public bool fixedPosition; // Can this item be picked up by the player or is fixed on the ground
    public MeshRenderer itemStatusIndicator; // The emissive mesh that indicate the item's status
    public Color defaultStatusColor; // The color of the indicator for default status;
    //public Color inRangeStatusColor; // The color of the indicator for the player's armTip is within using range status;
    public Color isUsingStatusColor; // The color of the indicator for the player is using (attached armTip to the item, but not controlling) status;
    public Color isControllingStatusColor; // The color of the indicator for the player is controlling (is pressing down the trigger) status;
    public Color unusableStatusColor; // The color of the indicator for not usable status;

    public float normalDrag; // The item's normal drag
    public float normalAngularDrag; // The item's normal angular drag
    public float normalMass; // The item's normal mass
    public bool isUsingGravity; // Does the item has gravity
    public bool isBeingHeld; // If this item is currently being held by an arm
    public Transform holdingArmTip; // The arm that is holding this item
    public Coroutine usingItem; // The coroutine that continuously triggers the using event
    public int timeUsed; // How many time has this item been used

    // Use this for initialization
    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            normalDrag = GetComponent<Rigidbody>().drag;
            normalAngularDrag = GetComponent<Rigidbody>().angularDrag;
            normalMass = GetComponent<Rigidbody>().mass;
            itemWeight = GetComponent<Rigidbody>().mass;
            isUsingGravity = GetComponent<Rigidbody>().useGravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculatingItemTransform();
    }

    /// <summary>
    /// Change the emissive color on the indicator
    /// </summary>
    /// <param name="newColor"></param>
    /// <param name="emissionIntensity"></param>
    public void ChangeIndicatorColor(Color newColor, float emissionIntensity)
    {
        // Change Albedo color
        itemStatusIndicator.material.color = newColor;
        // Change emissive color
        itemStatusIndicator.material.SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(newColor, 1));
    }

    public void CalculatingItemTransform()
    {
        if (isBeingHeld)
        {
            // If the item is not actually being hold by player arm
            if (holdingArmTip == null)
            {
                return;
            }
            // If the item can be lifted by the arm then match its transform with the armTip
            if (!fixedPosition && itemWeight <= holdingArmTip.GetComponentInParent<ControlArm>().armLiftingStrength)
            {
                //GetComponent<Rigidbody>().velocity = Vector3.zero;
                //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                transform.eulerAngles = new Vector3(0, holdingArmTip.GetComponentInParent<ControlArm>().joyStickRotationAngle, 0);
                //transform.position = holdingArm.position;
                //print(holdingArm.position);
            }
        }
    }

    public void ForceDropItem()
    {
        StopUsing(); // Stop using the item
        holdingArmTip.GetComponentInParent<ControlArm>().DropDownItem(gameObject);
    }

    /// <summary>
    /// Setup this item
    /// </summary>
    public void SetupItem()
    {
        setupEvent.Invoke();
    }

    /// <summary>
    /// Start using this item
    /// </summary>
    public void StartUsing()
    {
        // Change the status indicator color
        ChangeIndicatorColor(isControllingStatusColor, 1);

        usingItem = StartCoroutine(UsingItem());
    }

    /// <summary>
    /// Stop using this item
    /// </summary>
    public void StopUsing()
    {
        // If the user is using the item
        if (usingItem != null)
        {
            StopCoroutine(usingItem);

            // If there is an event for when the item is stopped being used, then triggers it
            if (stopUsingEvent != null)
            {
                stopUsingEvent.Invoke();
            }

            usingItem = null;

            // Change the status indicator color
            ChangeIndicatorColor(isUsingStatusColor, 1);
        }
    }

    public void ResetItem()
    {
        StopUsing(); // Stop using the item
        resetEvent.Invoke(); // Reset the item
    }

    /// <summary>
    /// The coroutine that keep using the item
    /// </summary>
    /// <returns></returns>
    public IEnumerator UsingItem()
    {
        // Keep using the item (maybe need to change "while (true)" to "while (still can be used)")
        while (true)
        {
            singleUseEvent.Invoke();

            timeUsed++; // Increase the counter of how many times it has been used
            // Stop the item from been used if it has a limited use and the limit is reached
            if (useLimit != 0 && useLimit == timeUsed)
            {
                canUse = false;
                StopCoroutine(usingItem);
            }

            if (eventCoolDown == 0)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(eventCoolDown);
            }
        }
    }
}
