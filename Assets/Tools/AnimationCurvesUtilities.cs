using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurvesUtilities 
{
    public static Vector3 EvaluateCurves(this AnimationCurve[] animationCurves, float time)
    {
        //Make sure that the AnimationCurve values are valid.
        if (animationCurves == null || animationCurves.Length != 3)
            return default;

        //Return.
        return new Vector3
        {
            //X.
            x = animationCurves[0].Evaluate(time),
            //Y.
            y = animationCurves[1].Evaluate(time),
            //Z.
            z = animationCurves[2].Evaluate(time)
        };
    }
}
