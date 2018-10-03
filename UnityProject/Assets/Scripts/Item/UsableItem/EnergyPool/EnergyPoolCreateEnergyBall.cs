using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// First Version
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


/* Second version
/// <summary>
/// Constantly fill up the energy ball pool if there isn't enough energy balls
/// </summary>
public class EnergyPoolCreateEnergyBall : MonoBehaviour
{
    public GameObject energyBall; // The energy ball prefab
    public int totalEnergyBallCapacity; // How much energy balls should be in the pool
    public Vector3[] energyBallCreatePositions; // Where can the energy balls appear

    public List<GameObject> currentEnergyBallInPool; // The energy balls that are currently in the pool

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If there are not enough energy balls in the pool
        if (currentEnergyBallInPool.Count <= totalEnergyBallCapacity - 3)
        {
            CreateEnergyBall();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If an energy ball is in the pool but is not kept track in the list
        if (!currentEnergyBallInPool.Contains(other.gameObject) && other.GetComponent<EnergyBallBehavior>())
        {
            currentEnergyBallInPool.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If an energy ball is leaving the pool and is kept track in the list
        if (currentEnergyBallInPool.Contains(other.gameObject))
        {
            currentEnergyBallInPool.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Create new energy ball
    /// </summary>
    public void CreateEnergyBall()
    {

    }
}
*/
