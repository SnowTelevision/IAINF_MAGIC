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
    public float showDelay; // How long the dialog should wait until shown

    public bool isShown; // Has this dialog been shown
    public static Coroutine showImageCoroutine; // The show image coroutine

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Start the delayed show image coroutine
    /// </summary>
    public void DelayedShowImage()
    {
        showImageCoroutine = StartCoroutine(DelayedShowImageCoroutine());
    }

    /// <summary>
    /// Show the dialog after some delay
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayedShowImageCoroutine()
    {
        yield return new WaitForSeconds(showDelay);

        ShowImage();
        showImageCoroutine = null;
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

            // Prevent the delayed show image shows after new image has been shown
            if (showImageCoroutine != null)
            {
                StopCoroutine(showImageCoroutine);
                showImageCoroutine = null;
            }

            GameManager.sGameManager.playerSelfDialogImage.sprite = imageToShow;

            if (!GameManager.sGameManager.playerSelfDialogImage.enabled)
            {
                GameManager.sGameManager.playerSelfDialogImage.enabled = true;
            }

            // If the dialog should auto hide
            if (autoHide)
            {
                StartCoroutine(DelayedHide());
            }

            // Change body particles
            GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.
                ChangeParticleSystemProfile(GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.bodyExcited);
            GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.bodyCurrentProfile =
                GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.bodyExcited;
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

            // Change body particles
            GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.
                ChangeParticleSystemProfile(GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.bodyDefault);
            GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.bodyCurrentProfile =
                GameManager.sPlayer.GetComponent<PlayerInfo>().bodyParticleManager.bodyDefault;
        }
    }
}
