using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ApplyMode { Override, Add }

public class MotionApplier : MonoBehaviour
{
    [Tooltip("Determines the way this component applies the values for all subscribed Motion components.")]
    [SerializeField]
    private ApplyMode applyMode;

    private  List<Motion> motions = new List<Motion>();

    private Transform thisTransform;
    private void Awake()
    {
        thisTransform = transform;
    }

    private void LateUpdate()
    {
            Vector3 finalLocation = default;
            Vector3 finaEulerAngles = default;
        motions.ForEach((motion =>
        {
            //Tick.
            motion.Tick();
            //Debug.Log("Motion");
            //Add Location.
            finalLocation += motion.GetLocation() * motion.Alpha;
            //Add Rotation.
            finaEulerAngles += motion.GetEulerAngles() * motion.Alpha;
        }));
        if (applyMode == ApplyMode.Override)
        {
            //Set Location.
            thisTransform.localPosition = finalLocation;
            //Set Euler Angles.
            thisTransform.localEulerAngles = finaEulerAngles;
        }
        //Add Mode.
        else if (applyMode == ApplyMode.Add)
        {
            //Add Location.
            thisTransform.localPosition += finalLocation;
            //Add Euler Angles.
            thisTransform.localEulerAngles += finaEulerAngles;
        }
    }
    public void Subscribe(Motion motion) => motions.Add(motion);

   
}
