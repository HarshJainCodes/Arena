using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to shift scene to MainMenu
/// </summary>
public class ToMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    /// <summary>
    /// For onclick listner on main menu button
    /// </summary>
    public static void ToMenuScript()
    {
        AudioManager.instance?.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance?.musicEventInstance.release();
        SceneManager.LoadScene("MainMenu");

    }

}

