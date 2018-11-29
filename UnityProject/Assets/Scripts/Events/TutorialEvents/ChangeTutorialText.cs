using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Show tutorial text when the player is in the trigger range
/// </summary>
public class ChangeTutorialText : MonoBehaviour
{
    public string tutorialTextToShow; // The tutorial text to be displayed

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Display the tutorial text
    /// </summary>
    public void DisplayText()
    {
        GameManager.sTutorialText.text = tutorialTextToShow;
        GameManager.sTutorialText.transform.parent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the tutorial text
    /// </summary>
    public void HideText()
    {
        GameManager.sTutorialText.transform.parent.gameObject.SetActive(false);
        GameManager.sTutorialText.text = "";
    }
}
