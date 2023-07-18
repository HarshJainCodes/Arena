using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PaletteChangeButton : MonoBehaviour
{
    [SerializeField] private int _SelfIndex=0;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(_SwitchPalette);
    }

    private void _SwitchPalette()
    {
        GetComponentInParent<PaleteUIMaster>().SwitchPanel(_SelfIndex);
    }
}
