using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public static float Linear(float a, float b, float t)
    {
        return b * t + a;
    }

    public static float InQuad(float a, float b, float t)
    {
        return b * Mathf.Pow(t, 2) + a;
    }

    public static float OutQuad(float a, float b, float t)
    {
        return -b * t * (t - 2) + a;
    }

    public static float InOutQuad(float a, float b, float t)
    {
        t *= 2;
        if (t < 1)
        {
            return b / 2 * Mathf.Pow(t, 2) + a;
        }
        return -b / 2 * ((t - 1) * (t - 3) - 1) + a;
    }

    public static float InBack(float a, float b, float t)
    {
        float s = 1.70158f;

        return b * t * t * ((s + 1) * t - s) + a;
    }

    public static float OutBack(float a, float b, float t)
    {
        float s = 1.70158f;

        t -= 1;
        return b * (t * t * ((s + 1) * t + s) + 1) + a;
    }

    public static float InOutBack(float a, float b, float t)
    {
        float s = 1.70158f * 1.525f;

        t *= 2;

        if (t < 1)
        {
            return b / 2 * (t * t * ((s + 1) * t - s)) + a;
        }
        t -= 2;
        return b / 2 * (t * t * ((s + 1) * t + s) + 2) + a;
    }
}
