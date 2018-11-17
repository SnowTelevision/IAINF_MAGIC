using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Change the player self dialog during different game event
/// </summary>
public class ChangeSelfDialogImage : MonoBehaviour
{
    public Sprite imageToShow; // The sprite of the dialog image to show
    public bool autoHide; // Does the dialog hide automatically after some time
    public float autoHideDelay; // How long does the dialog keep display before auto hide

    public bool isShown; // Has this dialog been shown

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Show the dialog image
    /// </summary>
    public void ShowImage()
    {
        // If this dialog has not been displayed
        if (!isShown)
        {
            isShown = true;

            GameManager.sGameManager.playerSelfDialogImage.sprite = imageToShow;

            if (!GameManager.sGameManager.playerSelfDialogImage.enabled)
            {
                GameManager.sGameManager.playerSelfDialogImage.enabled = true;
            }

            // If the dialog should auto hide
            if (autoHide)
            {
                DelayedHide();
            }
        }
    }

    /// <summary>
    /// Hide the dialog after some delay
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayedHide()
    {
        yield return new WaitForSeconds(autoHideDelay);

        HideImage();
    }

    /// <summary>
    /// Hide the dialog image
    /// </summary>
    public void HideImage()
    {
        // If the dialog is still showing this image and the dialog is enabled
        if (GameManager.sGameManager.playerSelfDialogImage.sprite == imageToShow &&
            GameManager.sGameManager.playerSelfDialogImage.enabled)
        {
            GameManager.sGameManager.playerSelfDialogImage.enabled = false;
        }
    }
}
