using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena {
    public class PlayerClimbing : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private PlayerMovement pm;
        [SerializeField]
        private LayerMask WhatIsWall;
        [SerializeField]
        private Transform orientation;

        private bool WallFront;
        private RaycastHit frontWall;
        private RaycastHit ledgeL;
        private RaycastHit ledgeR;
        private bool climbing;

        public float maxClimbTime;
        private float climbTimer;

        public float climbSpeed;
        private bool ledgeAvailableLeft;
        private bool ledgeAvailableRight;

        public Vector3 RayOffsetLeft;
        public Vector3 RayOffsetRight;

        Ray rayL;
        Ray rayR;

        [SerializeField]
        private Animator PlayerAnimator;

        [SerializeField]
        private LedgeGrabArms ledgeGrabArms;
  
        public float rayLength;

        public Transform LeftRaycastPoint;
        public Transform RightRaycastPoint;

        [SerializeField]
        private CameraLook cam;
        bool ledgeGrabbing;
        private void Update()
        {
            WallFront = Physics.SphereCast(transform.position,0.25f,orientation.forward,out frontWall,0.75f,WhatIsWall);
           

            ledgeAvailableLeft = Physics.Raycast(LeftRaycastPoint.position,Vector3.down,out ledgeL,rayLength,WhatIsWall);
            ledgeAvailableRight = Physics.Raycast(RightRaycastPoint.position,Vector3.down,out ledgeR,rayLength,WhatIsWall);

            Debug.DrawLine(LeftRaycastPoint.position, LeftRaycastPoint.position+Vector3.down*rayLength, Color.blue);
            Debug.DrawLine(RightRaycastPoint.position, RightRaycastPoint.position+Vector3.down*rayLength, Color.blue);


            if (ledgeAvailableLeft && ledgeAvailableRight && !ledgeGrabbing)
            {
            StartLedgeGrab();
            }
            else if((!ledgeAvailableLeft || !ledgeAvailableRight) && ledgeGrabbing)
            {
                StopLedgeGrab();
            }

            if (ledgeGrabbing) LedgeGrabMovement();

            StateMachine();
            if (climbing) ClimbingMovement();
            if (pm.IsPlayerGrounded()) climbTimer = maxClimbTime;
        }
      
        void StateMachine()
        {
            if(!pm.IsPlayerGrounded() && WallFront && Input.GetKey(KeyCode.W))
            {
                StartClimbing();
                if (climbTimer > 0) climbTimer -= Time.deltaTime;
                if (climbTimer < 0) StopClimbing();
            }
            else
            {
                StopClimbing();
            }
        }

        void StartClimbing()
        {
            climbing = true;
        }

        void ClimbingMovement()
        {
            rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        }
        void StopClimbing()
        {
            climbing = false;
        }
        Vector3 leftHandPoint;
        Vector3 rightHandPoint;

        [SerializeField]
        Vector3 lhandOffset;
        [SerializeField]
        Vector3 rhandOffset;
        void StartLedgeGrab()
        {
            Debug.Log("Start LG");
            PlayerAnimator.SetFloat("Play Rate Holster", 5);

            PlayerAnimator.SetBool("Holstered", true);
            pm.Ledgegrab = true;
            ledgeGrabbing = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            leftHandPoint = ledgeGrabArms.IK_Arm_Left_Target.transform.InverseTransformPoint(ledgeL.point);
            rightHandPoint = ledgeGrabArms.IK_Arm_Right_Target.transform.InverseTransformPoint(ledgeR.point);
            Debug.Log(orientation.forward);
            
            ledgeGrabArms.Ledge = true;
        }

        void StopLedgeGrab()
        {
            cam.DoTilt(10);
            StartCoroutine(ResetCamTilt());
            pm.Ledgegrab = false;
            Debug.Log("Stop LG");
            ledgeGrabbing = false;
            rb.useGravity = true;
            ledgeGrabArms.leftArmLedge.position = ledgeGrabArms.leftArm.position;
            ledgeGrabArms.rightArmLedge.position = ledgeGrabArms.rightArm.position;
            ledgeGrabArms.Ledge = false;
            PlayerAnimator.SetBool("Holstered", false);

        }

        void LedgeGrabMovement()
        {
            Debug.Log("LG motion");
            ledgeGrabArms.leftArmLedge.position = leftHandPoint - lhandOffset;
            ledgeGrabArms.rightArmLedge.position = rightHandPoint  -rhandOffset;
        }

        IEnumerator ResetCamTilt()
        {
            yield return new WaitForSeconds(0.25f);
            cam.DoTilt(0);

        }
    }
}
