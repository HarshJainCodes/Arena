using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used to generate a palete in the scene
/// </summary>
public class PalateGenerator : MonoBehaviour
{

    public void Awake()
    {

        int i = 0;
        var images = gameObject.GetComponentsInChildren<IndividualColour>();
        foreach (var item in images)
        {
            item.value = new Color(i / (float)images.Length, i / (float)images.Length, i / (float)images.Length);
            item.setParentColor();
            //Debug.Log(item.color);
            //PrefabUtility.RecordPrefabInstancePropertyModifications(item);
            i++;

            //Debug.Log(item.name);
        }
    }
}
