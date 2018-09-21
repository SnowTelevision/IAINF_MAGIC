using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an energy ball on player's armTip when the player's armTip reaches into it
/// </summary>
public class EnergyPoolCreateEnergyBall : MonoBehaviour
{
    public GameObject energyBall; // The energy ball prefab

    public List<EnergyBallBehavior> newlyCreatedEnergyBalls; // The array of energy balls that's being created but not activated

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (EnergyBallBehavior e in newlyCreatedEnergyBalls)
        {
            if (e.activated)
            {
                newlyCreatedEnergyBalls.Remove(e);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player's armTip stick into the energy pool
        if (other.GetComponent<ArmUseItem>())
        {
            CreateEnergyBallOnPlayerArmTip(other.transform); // Create a new energy ball
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If player's armTip that has the newly created energy ball get out of the energy pool
        if (other.GetComponent<ArmUseItem>())
        {
            foreach (EnergyBallBehavior e in newlyCreatedEnergyBalls)
            {
                // If the armTip that has a inactivate energy ball following it
                if (e.followingArmTip == other.transform)
                {
                    newlyCreatedEnergyBalls.Remove(e);
                    Destroy(e.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Creates an invisible energy ball on player's armTip's positions
    /// </summary>
    /// <param name="followingArmTip"></param>
    public void CreateEnergyBallOnPlayerArmTip(Transform followingArmTip)
    {
        EnergyBallBehavior newBall = Instantiate(energyBall).GetComponent<EnergyBallBehavior>();
        newBall.followingArmTip = followingArmTip;
        newlyCreatedEnergyBalls.Add(newBall);
    }
}
