using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPaleteMaster : MonoBehaviour
{
    [SerializeField] private ControlScript _controller;
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
