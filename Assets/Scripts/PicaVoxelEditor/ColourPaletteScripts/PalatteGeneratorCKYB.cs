using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using static UnityEditor.Progress;

public class PalatteGeneratorCKYB : MonoBehaviour
{
    private void Awake()
    {
        int i = 0;
        //for cmyk palette reference values
        float[,] cmykReference = new float[256, 4];
        float c = 0, m = 0, y = 0, k = 0, commonDifference = 0.25f;

        for (int j = 0; j < 256; j++)
        {
            if (k < 1)
            {
                k += commonDifference;
            }
            else
            {
                k = 0;
                if (y < 1)
                {
                    y += commonDifference;
                }
                else
                {
                    y = 0;
                    if (m < 1)
                    {
                        m += commonDifference;
                    }
                    else
                    {
                        m = 0;
                        if (c < 1)
                        {
                            c += commonDifference;
                        }
                        else
                        {
                            c = 0;
                        }
                    }
                }
            }


            cmykReference[j, 0] = c;
            cmykReference[j, 1] = m;
            cmykReference[j, 2] = y;
            cmykReference[j, 3] = k;


        }
        var images = gameObject.GetComponentsInChildren<IndividualColour>();
        foreach (var item in images)
        {
            int r = Convert.ToInt32(255 * (1 - cmykReference[i, 0]) * (1 - cmykReference[i, 3]));
            int g = Convert.ToInt32(255 * (1 - cmykReference[i, 1]) * (1 - cmykReference[i, 3]));
            int b = Convert.ToInt32(255 * (1 - cmykReference[i, 2]) * (1 - cmykReference[i, 3]));

            if (r == 0 && g == 0 && b == 0)
            {
                r = UnityEngine.Random.Range(1, 255);
                g = UnityEngine.Random.Range(1, 255);
                b = UnityEngine.Random.Range(1, 255);
            }

            item.value = new Color(r / (float)images.Length, g / (float)images.Length, b / (float)images.Length,1);
            //Debug.Log(r);
            //PrefabUtility.RecordPrefabInstancePropertyModifications(item);
            i++;
        }
    }
}
