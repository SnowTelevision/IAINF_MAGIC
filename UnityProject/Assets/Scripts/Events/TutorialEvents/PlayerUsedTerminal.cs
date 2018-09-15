using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check if the player used the first terminal
/// </summary>
public class PlayerUsedTerminal : MonoBehaviour
{
    public ItemInfo terminal; // The first terminal in Lv1Rm02
    public ChangeTutorialText tutorialTextTriggerToDestroy; // The tutorial text is showing

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the player start to use the terminal
        if (terminal.holdingArmTip != null)
        {
            Destroy(tutorialTextTriggerToDestroy.gameObject);
            Destroy(gameObject);
        }
    }
}
