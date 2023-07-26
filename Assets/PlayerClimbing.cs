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
        private Animator PlayerAnimator;
        [SerializeField]
        private CameraLook cam;
        [SerializeField]
        private LayerMask WhatIsWall;
        [SerializeField]
        private Transform orientation;
        [SerializeField]
        private Transform LeftRaycastPoint;
        [SerializeField]
        private Transform RightRaycastPoint;
        [SerializeField]
        private LedgeGrabArms ledgeGrabArms;

        [Header("Variables")]
        [SerializeField]
        private float maxClimbTime;
        [SerializeField]
        private float climbSpeed;
        [SerializeField]
        private Vector3 RayOffsetLeft;
        [SerializeField]
        private Vector3 RayOffsetRight;
        [SerializeField]
        private float rayLength;
        [SerializeField]
        private float ledgeJumpBackForce;
        [SerializeField]
        private float ledgeJumpUpForce;

        private bool WallFront;
        private RaycastHit frontWall;
        private RaycastHit ledgeL;
        private RaycastHit ledgeR;
        
        private bool climbing;
        bool ledgeGrabbing;

        private float climbTimer;

        private bool ledgeAvailableLeft;
        private bool ledgeAvailableRight;

        Ray rayL;
        Ray rayR;

        Vector3 leftHitPoint;
        Vector3 rightHitPoint;
        
        private void Update()
        {
            WallFront = Physics.SphereCast(transform.position,0.25f,orientation.forward,out frontWall,0.75f,WhatIsWall);

            ledgeAvailableLeft = Physics.Raycast(LeftRaycastPoint.position, Vector3.down, out ledgeL, rayLength, WhatIsWall);
            ledgeAvailableRight = Physics.Raycast(RightRaycastPoint.position, Vector3.down, out ledgeR, rayLength, WhatIsWall);

            if(ledgeAvailableLeft && ledgeAvailableRight)
            {
                
                float playerDisplacement = ledgeL.distance-0.1f;
                if(!ledgeGrabbing)
                {

                    /*leftHitPoint = ledgeL.point;
                    rightHitPoint = ledgeR.point;*/
                    Vector3 playerOrigPos = pm.transform.position;
                    Vector3 playerNewPos = pm.transform.position - new Vector3(0, playerDisplacement, 0);
                    Debug.Log("Hi");
                    StartCoroutine(MoveToPosition(playerNewPos, playerOrigPos, 0.2f));
                    
                    StartLedgeGrab();

                }
            }
            else if((!ledgeAvailableLeft || !ledgeAvailableRight) && ledgeGrabbing)
            {
                StopLedgeGrab();
            }

            

            
               /* ledgeAvailableLeft = Physics.SphereCast(LeftRaycastPoint.position, 0.05f, orientation.forward, out ledgeL, 1f, WhatIsWall);
                ledgeAvailableRight = Physics.SphereCast(RightRaycastPoint.position, 0.05f, orientation.forward, out ledgeR, 1f, WhatIsWall);*/
            
                Debug.DrawLine(LeftRaycastPoint.position, LeftRaycastPoint.position+Vector3.down*rayLength, Color.blue);
            Debug.DrawLine(RightRaycastPoint.position, RightRaycastPoint.position+Vector3.down*rayLength, Color.blue);


           /* if (ledgeAvailableLeft && ledgeAvailableRight && !ledgeGrabbing)
            {
            StartLedgeGrab();
            }
            else if((!ledgeAvailableLeft || !ledgeAvailableRight) && ledgeGrabbing)
            {
                StopLedgeGrab();
            }*/

            if (ledgeGrabbing) LedgeGrabMovement();

            StateMachine();
            if (climbing) ClimbingMovement();
            if (pm.IsPlayerGrounded()) climbTimer = maxClimbTime;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(LeftRaycastPoint.position, 0.05f);
            Gizmos.DrawSphere(RightRaycastPoint.position, 0.05f);

            Gizmos.DrawSphere(ledgeL.point,0.01f);
            Gizmos.DrawSphere(ledgeR.point,0.01f);
        }
        void StateMachine()
        {
            if(!pm.IsPlayerGrounded() && WallFront && Input.GetKey(KeyCode.W) && !pm.isWallRunning)
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
            //PlayerAnimator.SetBool("Holstered", true);
           // Debug.Log("start climbing");
        }

        void ClimbingMovement()
        {
            rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        }
        void StopClimbing()
        {
            climbing = false;
            //
            //  PlayerAnimator.SetBool("Holstered", false);

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
            cam.DoTilt(-10);

            PlayerAnimator.SetFloat("Play Rate Holster", 5);

            PlayerAnimator.SetBool("Holstered", true);
           
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            leftHandPoint = ledgeGrabArms.IK_Arm_Left_Target.transform.InverseTransformPoint(LeftRaycastPoint.position);
            //leftHandPoint = ledgeGrabArms.IK_Arm_Left_Target.transform.InverseTransformPoint(LeftRaycastPoint.position);
            rightHandPoint = ledgeGrabArms.IK_Arm_Right_Target.transform.InverseTransformPoint(RightRaycastPoint.position);
            //rightHandPoint = ledgeGrabArms.IK_Arm_Right_Target.transform.InverseTransformPoint(RightRaycastPoint.position);
            Debug.Log(orientation.forward);
            StartCoroutine(ResetCamTilt());

            ledgeGrabArms.Ledge = true;
            pm.Ledgegrab = true;
            ledgeGrabbing = true;
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
            if(Input.GetKeyDown(KeyCode.Space))
            {
                LedgeJump();
            }
        }

        IEnumerator ResetCamTilt()
        {
            yield return new WaitForSeconds(0.25f);
            cam.DoTilt(0);

        }

        void LedgeJump()
        {
            rb.velocity = Vector3.zero;
            Vector3 forceToApply = Vector3.up * ledgeJumpUpForce + (-orientation.forward * ledgeJumpBackForce);
            rb.AddForce(forceToApply, ForceMode.Impulse);
            StopLedgeGrab();
        }

        IEnumerator MoveToPosition(Vector3 newPos, Vector3 startPos,float time)
        {
            float elapsedTime = 0f;
            Debug.Log("Coroutine started");

            while (elapsedTime<time)
            {
                pm.transform.position = Vector3.Lerp(startPos, newPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            pm.transform.position = newPos;
            Debug.Log("Coroutine Completed");
        }
    }
}
