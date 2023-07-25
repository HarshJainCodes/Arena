using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LedgeTransform
{
    public Vector3 position;
    public Vector3 Rotation;
}
public class LedgeGrabArms : MonoBehaviour
{
    public LedgeTransform leftArmLedge;
    public LedgeTransform rightArmLedge;

    public LedgeTransform leftArm;
    public LedgeTransform rightArm;



    public bool Ledge;
    [SerializeField]
    public Transform IK_Arm_Left_Target;
    [SerializeField]
    public Transform IK_Arm_Right_Target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Ledge)
        {
            IK_Arm_Right_Target.localPosition = rightArmLedge.position;
            IK_Arm_Left_Target.localPosition = leftArmLedge.position;

            IK_Arm_Left_Target.localRotation = Quaternion.Euler(leftArmLedge.Rotation);
            IK_Arm_Right_Target.localRotation = Quaternion.Euler(rightArmLedge.Rotation);
        }
        else
        {

            IK_Arm_Right_Target.localPosition = rightArm.position;
            IK_Arm_Left_Target.localPosition = leftArm.position;

            IK_Arm_Left_Target.localRotation = Quaternion.Euler(leftArm.Rotation);
            IK_Arm_Right_Target.localRotation = Quaternion.Euler(rightArm.Rotation);


        }
    }
}
