using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Open the door when the room has no energy
/// </summary>
public class DoorOpenWhenNoEnergy : MonoBehaviour
{
    public EnergyBeam_Wide_Behavior[] wideEnergyBeams; // The wide energy beams that need to overload the room's energy
    public SlidingDoor[] slidingDoors; // The sliding doors to be opened after energy overload

    public bool overloadProcessStarted; // Has the energy overloading process started

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!overloadProcessStarted)
        {
            if (CheckOverloadedEnergyBeams())
            {
                overloadProcessStarted = true;
                StartEnergyOverloadProcess();
            }
        }
    }

    /// <summary>
    /// Check how many energy beams are overloaded
    /// </summary>
    /// <returns></returns>
    public bool CheckOverloadedEnergyBeams()
    {
        foreach (EnergyBeam_Wide_Behavior e in wideEnergyBeams)
        {
            if (!e.isOverloaded)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Start the overload process
    /// </summary>
    void StartEnergyOverloadProcess()
    {
        foreach (SlidingDoor s in slidingDoors)
        {
            s.OpenDoor();
        }
    }
}
