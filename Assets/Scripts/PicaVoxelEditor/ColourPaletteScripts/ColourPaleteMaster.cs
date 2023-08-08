using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script acts as a master script for the 4 colour palettes present in the scene
/// </summary>
public class ColourPaleteMaster : MonoBehaviour
{
    /// <summary>
    /// Handle to the master control script that connects to the master Voxel Runtimer editor script
    /// </summary>
    [SerializeField] private ControlScript _controller;
    /// <summary>
    /// Holds a colour which is passed to the above script.
    /// </summary>
    private IndividualColour _pickedButton;

    public void passColour(Color color)
    {
        _controller.ColourSet(color);
    }

    public void passColour(Color color,IndividualColour obj)
    {
        _pickedButton = obj;
        _controller.ColourSet(color);
    }

    public void SetPickedButtonColor(Color value)
    {
        if (_pickedButton != null)
        {
            _pickedButton.value = value;
            _pickedButton.setParentColor();
            Debug.Log("value");
        }
    }
}
