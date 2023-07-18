using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaleteUIMaster : MonoBehaviour
{
    [SerializeField] private GameObject[] _ColorPaletes;
    [SerializeField] private int _Index=0;
    //[SerializeField] private GameObject _ColourPicker;

    public void Awake()
    {
        for(int i=0;i<_ColorPaletes.Length;i++)
        {
            _ColorPaletes[i].SetActive(false);
        }
        _ColorPaletes[_Index].SetActive(true);
        //_ColourPicker.SetActive(false);
    }
    public void SwitchPanel(int index)
    { 
        _ColorPaletes[_Index].SetActive(false);
        _Index = index;
        _ColorPaletes[index].SetActive(true);
        /*if(index==3)
        {
            _ColourPicker.SetActive(true);
        }*/
    }
}
