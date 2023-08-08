using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to shift scene to MainMenu
/// </summary>
public class ToMenu : MonoBehaviour
{
    /// <summary>
    /// For onclick listner on main menu button
    /// </summary>
    public static void ToMenuScript()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

