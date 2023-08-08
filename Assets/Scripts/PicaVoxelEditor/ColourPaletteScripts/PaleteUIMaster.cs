using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Master component that swaps between the various palettes
/// </summary>
public class PaleteUIMaster : MonoBehaviour
{
    /// <summary>
    /// Store the various color palettes to switch between them.
    /// </summary>
    [SerializeField] private GameObject[] _ColorPaletes;
    /// <summary>
    /// Stores the index of current color palette
    /// </summary>
    [SerializeField] private int _Index=0;
    /// <summary>
    /// Handle to Colour Picker object which can set the color of the palete element.
    /// </summary>
    [SerializeField] private GameObject _ColourPicker;

    public void Awake()
    {
        for(int i=0;i<_ColorPaletes.Length;i++)
        {
            _ColorPaletes[i].SetActive(false);
        }
        _ColorPaletes[_Index].SetActive(true);
        _ColourPicker.SetActive(false);
    }
    /// <summary>
    /// Function to switch between paletes.
    /// </summary>
    /// <param name="index"></param>
    public void SwitchPanel(int index)
    { 
        _ColorPaletes[_Index].SetActive(false);
        _Index = index;
        _ColorPaletes[index].SetActive(true);
        if (index == 3)
        {
            _ColourPicker.SetActive(true);
        }
        else
        {
            _ColourPicker.SetActive(false);
        }
    }
}
