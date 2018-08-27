using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemInfo : MonoBehaviour
{
    public bool canUse; // Is this an usable item? If it is not then the player has to keep holding down the grab item button to hold it
    public float itemWeight; // The item's weight. If it is smaller or equal than how much the arm can carry, it should be able to be lifted by the arm and not touching the ground
    public UnityEvent singleUseEvent; // An event to be triggered for a single use of the item
    public float eventCoolDown; // When the player is holding down the use button and constantly using the item, how long it should wait before each use

    public float normalDrag; // The item's normal drag
    public float normalAngularDrag; // The item's normal angular drag
    public float normalMass; // The item's normal mass
    public bool isBeingHeld; // If this item is currently being held by an arm
    public Transform holdingArm; // The arm that is holding this item
    public Coroutine usingItem; // The coroutine that continuously triggers the using event

    // Use this for initialization
    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            normalDrag = GetComponent<Rigidbody>().drag;
            normalAngularDrag = GetComponent<Rigidbody>().angularDrag;
            normalMass = GetComponent<Rigidbody>().mass;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculatingItemTransform();
    }

    public void CalculatingItemTransform()
    {
        if (isBeingHeld)
        {
            // If the item can be lifted by the arm then match its transform with the armTip
            if (itemWeight <= holdingArm.GetComponentInParent<ControlArm>().armLiftingStrength)
            {
                //GetComponent<Rigidbody>().velocity = Vector3.zero;
                //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                transform.eulerAngles = new Vector3(0, holdingArm.GetComponentInParent<ControlArm>().joyStickRotationAngle, 0);
                //transform.position = holdingArm.position;
                //print(holdingArm.position);
            }
        }
    }

    private void OnJointBreak(float breakForce)
    {
        ForceDropItem();
    }

    public void ForceDropItem()
    {
        holdingArm.GetComponentInParent<ControlArm>().DropDownItem(gameObject);
    }

    /// <summary>
    /// Start using this item
    /// </summary>
    public void StartUsing()
    {
        usingItem = StartCoroutine(UsingItem());
    }

    /// <summary>
    /// Stop using this item
    /// </summary>
    public void StopUsing()
    {
        if (usingItem != null)
        {
            StopCoroutine(usingItem);
            usingItem = null;
        }
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
