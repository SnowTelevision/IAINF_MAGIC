using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Store some global variebles and some global functions
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject player; // The player's gameobject
    public TextMeshProUGUI tutorialText; // The tutorial text

    public static GameObject sPlayer; // The static reference of the player's gameobject
    public static bool gamePause; // Is the game paused or not
    public static bool inScriptedEvent; // Is the game currently in some scripted event where the player don't have free control over the character
    public static TextMeshProUGUI sTutorialText; // Static ref

    // Use this for initialization
    void Start()
    {
        sPlayer = player;
        sTutorialText = tutorialText;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        gamePause = true;

    }

    /// <summary>
    /// When a scripted event start
    /// </summary>
    public static void ScriptedEventStart()
    {
        inScriptedEvent = true;
    }

    public void NoStaticScriptedEventStart()
    {
        GameManager.ScriptedEventStart();
    }

    /// <summary>
    /// When a scripted event stop
    /// </summary>
    public static void ScriptedEventStop()
    {
        inScriptedEvent = false;
    }

    public void NoStaticScriptedEventStop()
    {
        GameManager.ScriptedEventStop();
    }
}
