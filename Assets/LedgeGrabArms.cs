using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//struct to store position and rotation values to apply to ARM IK BONES 
[System.Serializable]
public struct LedgeTransform
{
    public Vector3 position;
    public Vector3 Rotation;
}
public class LedgeGrabArms : MonoBehaviour
{

    [SerializeField]
    public Transform IK_Arm_Left_Target;
    [SerializeField]
    public Transform IK_Arm_Right_Target;

    public LedgeTransform leftArmLedge; //Position and Rotation values the LeftArm needs to be while ON LEDGE
    public LedgeTransform rightArmLedge; //Position and Rotation values the RightArm needs to be while ON LEDGE

    public LedgeTransform leftArm;//Position and Rotation values the LeftArm needs to be while NOT ON LEDGE
    public LedgeTransform rightArm;//Position and Rotation values the RightArm needs to be while NOT ON LEDGE


    //The lerping speed for the hands to change position
    public float speed;
    //true if on ledge
    public bool Ledge;
   
    void Update()
    {
        //Sets Position and Rotation to both arms while ON LEDGE
        if(Ledge)
        {
            IK_Arm_Right_Target.localPosition = Vector3.Lerp(IK_Arm_Right_Target.localPosition, rightArmLedge.position,0.8f*Time.deltaTime*speed);
            IK_Arm_Left_Target.localPosition = Vector3.Lerp(IK_Arm_Left_Target.localPosition, leftArmLedge.position,0.8f*Time.deltaTime*speed);

            IK_Arm_Left_Target.localRotation = Quaternion.Euler(leftArmLedge.Rotation);
            IK_Arm_Right_Target.localRotation = Quaternion.Euler(rightArmLedge.Rotation);
        }
        //Sets Position and Rotation to both arms while NOT ON LEDGE
        else
        {

            IK_Arm_Right_Target.localPosition = rightArm.position;
            IK_Arm_Left_Target.localPosition = leftArm.position;

            IK_Arm_Left_Target.localRotation = Quaternion.Euler(leftArm.Rotation);
            IK_Arm_Right_Target.localRotation = Quaternion.Euler(rightArm.Rotation);


        }
    }
}
