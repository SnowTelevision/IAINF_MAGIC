using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarrierGenerator : MonoBehaviour
{
    public bool isWeakBarrier; // Is this barrier a weak one that has a spawn of two units
    public GameObject leftGenerator; // The generator on the left side
    public GameObject rightGenerator; // The generator on the right side
    public GameObject middleGenerator; // The generator in the middle
    public GameObject barrier; // The model of the energy barrier

    // Use this for initialization
    void Start()
    {
        // Turn off the weak barrier by default
        if (isWeakBarrier)
        {
            barrier.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy this barrier if the generator on the left or right side is inactive (collide by the mechanical arm hand)
        if (!leftGenerator.activeInHierarchy || !rightGenerator.activeInHierarchy)
        {
            Destroy(gameObject);
        }

        // Turn on the weak barrier if only its middle generator is destroyed
        if (isWeakBarrier && !middleGenerator.activeInHierarchy)
        {
            barrier.SetActive(true);
        }
    }
}
