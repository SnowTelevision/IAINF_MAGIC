using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

/// <summary>
/// Scientist's behavior when it is being attacked by the player
/// </summary>
public class ScientistBeingAttack : MonoBehaviour
{
    public float killTime; // How long does it take for the player to kill the scientist
    public float singleArmHoldTime; // How long can the scientist be held by only one arm before escape
    public CountDeadScientistToDropKey deadScientistCounter; // The counter that keep tracks of the dead scientists

    public List<GameObject> touchingArms; // The player's arm(s) that is currently within the attack range
    public bool isDead; // Is this scientist dead
    public float firstArmStartTouchingTime; // When does the player start holding the scientist with one arm
    public float bothArmStartHoldingTime; // When does the player start holding with both arms

    // Use this for initialization
    void Start()
    {
        // Set the escape time is the scientist is touched by the player for too long
        GetComponents<BehaviorTree>()[1].SetVariableValue("EscapeTime", singleArmHoldTime);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAttackStatus();

        // Set the other times
        GetComponents<BehaviorTree>()[1].SetVariableValue("FirstArmStartTouchingTime", firstArmStartTouchingTime);
        GetComponents<BehaviorTree>()[1].SetVariableValue("BothArmStartHoldingTime", bothArmStartHoldingTime);
    }

    private void OnTriggerStay(Collider other)
    {
        // If an arm enters the attack range
        if (other.GetComponent<ArmUseItem>())
        {
            if (touchingArms.Count == 0)
            {
                firstArmStartTouchingTime = Time.time;
            }

            if (!touchingArms.Contains(other.gameObject))
            {
                touchingArms.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If an arm leaves the attack range
        if (other.GetComponent<ArmUseItem>())
        {
            // "Detach" the armTip on the scientist
            if (other.GetComponent<FixedJoint>())
            {
                Destroy(other.GetComponent<FixedJoint>());
                other.GetComponent<ArmUseItem>().currentlyHoldingItem = null;
            }

            touchingArms.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Update the status relate to scientist been attacked
    /// </summary>
    public void UpdateAttackStatus()
    {
        // Reset timer when the player is not attacking the scientist
        if (touchingArms.Count == 0)
        {
            firstArmStartTouchingTime = 0;
        }

        // Count how many player arms is holding the scientist
        float holdingArmCount = 0;

        foreach (GameObject g in touchingArms)
        {
            // If the player is holding down trigger to hold the scientist
            if (!g.GetComponent<ArmUseItem>().hasTriggerReleased)
            {
                // "Attach" the armTip on the scientist
                if (!g.GetComponent<FixedJoint>())
                {
                    g.GetComponent<ArmUseItem>().currentlyHoldingItem = gameObject;
                    FixedJoint newJoint = g.AddComponent<FixedJoint>();
                }

                holdingArmCount++;
            }
            else
            {
                // "Detach" the armTip on the scientist
                if (g.GetComponent<FixedJoint>())
                {
                    Destroy(g.GetComponent<FixedJoint>());
                    g.GetComponent<ArmUseItem>().currentlyHoldingItem = null;
                }
            }
        }

        // Start timer when the player start holding the scientist with both arms
        if (bothArmStartHoldingTime == 0 && holdingArmCount == 2)
        {
            bothArmStartHoldingTime = Time.time;
        }

        // Reset the holding time when the player stop holding the scientist with two arms
        if (holdingArmCount < 2)
        {
            bothArmStartHoldingTime = 0;
        }

        // If the player hold the scientist for long enough
        if (bothArmStartHoldingTime != 0 && Time.time - bothArmStartHoldingTime >= killTime)
        {
            isDead = true;
            GetComponents<BehaviorTree>()[1].DisableBehavior();
            //GetComponent<CapsuleCollider>().isTrigger = false;
            Destroy(GetComponent<CapsuleCollider>());
            deadScientistCounter.deadScientistCount++;
            GetComponentInChildren<Animator>().SetBool("PlayDead", true);

            // If this scientist is the last one needed for the key to be dropped
            if (!deadScientistCounter.keyDropped && deadScientistCounter.deadScientistCount == deadScientistCounter.numberRequireToDropKey)
            {
                // Drop the key at this scientist's position
                deadScientistCounter.DropKey(transform.position + Vector3.up * 2);
            }

            // "Detach" the "attached" armTip on the scientist when the scientist is dead
            foreach (GameObject g in touchingArms)
            {
                // "Detach" the armTip on the scientist
                if (g.GetComponent<FixedJoint>())
                {
                    Destroy(g.GetComponent<FixedJoint>());
                    g.GetComponent<ArmUseItem>().currentlyHoldingItem = null;
                }

                GetComponent<ItemInfo>().ForceDropItem();
            }

            this.enabled = false;
        }
    }
}
