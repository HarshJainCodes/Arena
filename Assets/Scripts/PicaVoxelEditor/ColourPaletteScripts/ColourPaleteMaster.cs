using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPaleteMaster : MonoBehaviour
{
    [SerializeField] private ControlScript _controller;

    public void passColour(Color color)
    {
        _controller.ColourSet(color);
    }
}
