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
        private PlayerMovement pm; // ref to PlayerMovement.cs script
        [SerializeField]
        private Animator PlayerAnimator; // ref to Animator
        [SerializeField]
        private CameraLook cam;
        [SerializeField]
        private LayerMask WhatIsWall;
        [SerializeField]
        private Transform orientation;
        [SerializeField]
        private Transform LeftRaycastPoint; //point from where the raycast will start for LEFT hand
        [SerializeField]
        private Transform RightRaycastPoint;//point from where the raycast will start for RIGHT hand
        [SerializeField]
        private LedgeGrabArms ledgeGrabArms;

        [Header("Variables")]
        [SerializeField]
        private float maxClimbTime; // controls how long the player can climb
        [SerializeField]
        private float climbSpeed; // controls the climbing speed
        [SerializeField]
        private float rayLength; //controls the raycast length
        [SerializeField]
        private float ledgeJumpBackForce; //the amount of force that will be applied in the back direction
        [SerializeField]
        private float ledgeJumpUpForce;//the amount of force that will be applied in the up direction
        [SerializeField]
        Vector3 lhandOffset;//offset values to correct LEFT hand positions
        [SerializeField]
        Vector3 rhandOffset;//offset valies to correct RIGHT hand positions

        private bool WallFront; //checks if we have wall in front for climbing
        private RaycastHit frontWall;
        private RaycastHit ledgeL; //stores left hand ledge hit properties
        private RaycastHit ledgeR; //store right hand ledge hit properties
        
        private bool climbing; //true if player is climbing
        bool ledgeGrabbing; // true if players is ledgeGrabbing

        private float climbTimer; //to keep track for climbing timer

        private bool ledgeAvailableLeft; //if ledge is available for left hand
        private bool ledgeAvailableRight; //if ledge is available for right hand

        Vector3 leftHandPoint;
        Vector3 rightHandPoint;


        bool canLedgeGrab = true;

        private void Update()
        {
            WallFront = Physics.SphereCast(transform.position,0.25f,orientation.forward,out frontWall,0.75f,WhatIsWall);

            ledgeAvailableLeft = Physics.Raycast(LeftRaycastPoint.position, Vector3.down, out ledgeL, rayLength, WhatIsWall);
            ledgeAvailableRight = Physics.Raycast(RightRaycastPoint.position, Vector3.down, out ledgeR, rayLength, WhatIsWall);

            if(ledgeAvailableLeft && ledgeAvailableRight && Input.GetKey(KeyCode.W))
            {
                canLedgeGrab = false;
                ClimbUpTheLedge();
                Invoke("ResetCanLedgeGrab", 0.5f);

            }
            else if(ledgeAvailableLeft && ledgeAvailableRight && canLedgeGrab)
            {
                StopClimbing();
                Debug.Log(ledgeL.distance);
                //calculate the amount the players will have to displace in order to display ledge grab properly.
                float playerDisplacement = ledgeL.distance-0.1f;
                if(!ledgeGrabbing)
                {
                    Vector3 playerOrigPos = pm.transform.position; //store player old Pos
                    Vector3 playerNewPos = pm.transform.position - new Vector3(0, playerDisplacement, 0); //calculate player new position
                    
                    StartCoroutine(MoveToPosition(playerNewPos, playerOrigPos, 0.2f)); // move player to new position
                    
                    //start the ledge grab
                    if(canLedgeGrab)
                        StartLedgeGrab();

                }
            }
            else if((!ledgeAvailableLeft || !ledgeAvailableRight) && ledgeGrabbing)
            {
                StopLedgeGrab();
            }


            //debugs for raycasts
            Debug.DrawLine(LeftRaycastPoint.position, LeftRaycastPoint.position+Vector3.down*rayLength, Color.blue);
            Debug.DrawLine(RightRaycastPoint.position, RightRaycastPoint.position+Vector3.down*rayLength, Color.blue);

            if (ledgeGrabbing) LedgeGrabMovement();//perfrom the actual ledge grab movement

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
        /// <summary>
        /// Checks the current state of player if he's climbing or not
        /// </summary>
        void StateMachine()
        {
            if(!pm.IsPlayerGrounded() && WallFront && Input.GetKey(KeyCode.W) && !pm.isWallRunning &&!ledgeGrabbing)
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
        /// <summary>
        /// Adds velocity in up direction to player
        /// </summary>
        void ClimbingMovement()
        {
            rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        }
        void StopClimbing()
        {
            climbing = false;
        }
        /// <summary>
        /// Starts the ledge grab movement, sets gravity,bools, animator parameters, left and right hand positions.
        /// </summary>
        void StartLedgeGrab()
        {
            

            Debug.Log("Start LG");
            cam.DoTilt(-10);
            StopClimbing();
            PlayerAnimator.SetFloat("Play Rate Holster", 5);

            PlayerAnimator.SetBool("Holstered", true);
           
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            leftHandPoint = ledgeGrabArms.IK_Arm_Left_Target.transform.InverseTransformPoint(LeftRaycastPoint.position);
            rightHandPoint = ledgeGrabArms.IK_Arm_Right_Target.transform.InverseTransformPoint(RightRaycastPoint.position);
            StartCoroutine(ResetCamTilt());
            ledgeGrabArms.Ledge = true;
            pm.Ledgegrab = true;
            ledgeGrabbing = true;
        }
        /// <summary>
        /// Resets all bools checks, animator parameters, left and right hand positions.
        /// </summary>
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

        /// <summary>
        /// Keeps the hands at the required position and checks for jump input
        /// </summary>
        void LedgeGrabMovement()
        {
            Debug.Log("LG motion");
            ledgeGrabArms.leftArmLedge.position = leftHandPoint - lhandOffset;
            ledgeGrabArms.rightArmLedge.position = rightHandPoint  -rhandOffset;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                LedgeJump();
            }
            if(Input.GetKey(KeyCode.W) && ledgeGrabbing)
            {
                canLedgeGrab = false;
                
                ClimbUpTheLedge();
                Invoke("ResetCanLedgeGrab", 0.5f);
            }
        }

        /// <summary>
        /// Coroutine function to Reset camera motion
        /// </summary>
        /// <returns></returns>
        IEnumerator ResetCamTilt()
        {
            yield return new WaitForSeconds(0.25f);
            cam.DoTilt(0);
        }
        /// <summary>
        /// Performs the LedgeJump
        /// </summary>
        void LedgeJump()
        {
            rb.velocity = Vector3.zero;
            //calculates the force that needs to be applied.
            Vector3 forceToApply = Vector3.up * ledgeJumpUpForce + (-orientation.forward * ledgeJumpBackForce);
            rb.AddForce(forceToApply, ForceMode.Impulse);
            StopLedgeGrab();
        }
        
        void ClimbUpTheLedge()
        {
            rb.velocity = Vector3.zero;
            Vector3 upforceToApply = Vector3.up * 15;
            rb.AddForce(upforceToApply, ForceMode.Impulse);
            Vector3 frontForceToApply = orientation.forward * 20;
            rb.AddForce(frontForceToApply, ForceMode.Impulse);
            StopLedgeGrab();
        }

        /// <summary>
        /// Lerps player to the gives newPos from startPos in time t
        /// </summary>
        /// <param name="newPos"></param>
        /// <param name="startPos"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        IEnumerator MoveToPosition(Vector3 newPos, Vector3 startPos,float t)
        {
            float elapsedTime = 0f;
            Debug.Log("Coroutine started");

            while (elapsedTime<t)
            {
                pm.transform.position = Vector3.Lerp(startPos, newPos, (elapsedTime / t));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            pm.transform.position = newPos;
            Debug.Log("Coroutine Completed");
        }
        void ResetCanLedgeGrab()
        {
            canLedgeGrab = true;
        }
    }
}
