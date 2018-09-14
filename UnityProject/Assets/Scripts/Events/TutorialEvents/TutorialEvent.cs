using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basic structure for tutorial events
/// </summary>
public class TutorialEvent : MonoBehaviour
{
    public GameObject currentTutorialTextTrigger; // The current tutorial text is showing
    public GameObject nextTutorialTextTrigger; // The next cued tutorial text to show after the current event

    public bool eventFinished; // If this tutorial event is finished

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (eventFinished)
        {
            Destroy(currentTutorialTextTrigger);
            nextTutorialTextTrigger.SetActive(true);
            Destroy(gameObject);
        }
    }
}
