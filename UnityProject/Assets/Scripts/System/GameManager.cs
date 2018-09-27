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
    public static TextMeshProUGUI sTutorialText; // Static reference for the tutorial UI area

    // Test
    public bool test; // Is testing
    public bool deleteSave; // If we delete the save when the game start

    // Use this for initialization
    void Start()
    {
        if (test && deleteSave)
        {
            ES3.DeleteKey("PlayerPosition");
        }

        sPlayer = player;
        sTutorialText = tutorialText;

        // If there is a save for the current chapter
        if (ES3.KeyExists("PlayerPosition"))
        {
            player.transform.position = ES3.Load<Vector3>("PlayerPosition");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            print(gamePause);
        }
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public static void PauseGame()
    {
        Time.timeScale = 0;
        gamePause = true;
    }

    /// <summary>
    /// Unpause the game
    /// </summary>
    public static void UnpauseGame()
    {
        Time.timeScale = 1;
        gamePause = false;
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
