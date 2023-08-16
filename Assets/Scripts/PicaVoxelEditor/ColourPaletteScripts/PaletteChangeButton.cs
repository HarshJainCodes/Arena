using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the onclick script for palette change
/// </summary>
[RequireComponent(typeof(Button))]
public class PaletteChangeButton : MonoBehaviour
{
    /// <summary>
    /// Script to store self index
    /// </summary>
    [SerializeField] private int _SelfIndex=0;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(_SwitchPalette);
    }

    /// <summary>
    /// Function to switch the palette. This one goes on a button.
    /// </summary>
    private void _SwitchPalette()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.Select, this.transform.position);
        GetComponentInParent<PaleteUIMaster>().SwitchPanel(_SelfIndex);
    }
}
