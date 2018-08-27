using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<Actions>().Aiming();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
