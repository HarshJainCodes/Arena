using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Arena
{
    public class PlayerWallRun : MonoBehaviour
    {
        [Header("References")]
        public Transform orientation;
        private PlayerMovement pm;
        private Rigidbody rb;

        [Header("Parameters")]
        public bool CanWallRunInfinity = false;
        public float maxWallRunTime;
        private float wallRunTimer;
        public bool useGravity;

        [Header("Keybinds")]
        public KeyCode wallJumpKey;

        [Header("Wall Running")]
        public LayerMask whatIsWall;
        public LayerMask whatIsGround;
        public float wallRunForce;
        public float wallJumpUpForce;
        public float wallJumpSideForce;

        [Header("Exiting")]
        private bool exitingWall;
        public float exitWallTime;
        private float exitWallTimer;

        [Header("Detection")]
        public float wallCheckDistance;
        public float minJumpHeight;
        private RaycastHit leftWallHit;
        private RaycastHit rightWallHit;
        private bool wallLeft;
        private bool wallRight;

        private Vector2 keyInput;
        [SerializeField]
        private CameraLook cam;

        [SerializeField]
        private LeaningInput leaningInput;

        #region UNITY FUNCTION
        private void Start()
        {
            pm = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody>();

        }

        private void Update()
        {
            CheckForWall();
            StateMachine();
        }
        private void FixedUpdate()
        {
            if (pm.isWallRunning)
                WallRunning();
        }
        #endregion

        #region CUSTOM FUNCTIONS
        /// <summary>
        /// Checks if player has a wall to his left or his right
        /// </summary>
        private void CheckForWall()
        {
            Vector3 RightDirection = transform.position + orientation.right + orientation.forward;
            Vector3 LeftDirection = transform.position + -orientation.right + orientation.forward;
            wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
            wallLeft = Physics.Raycast(transform.position, -orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        }

        /// <summary>
        /// Checks if player is at a certain height to wallrun
        /// </summary>
        /// <returns></returns>
        private bool AboveGround()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
        }
        /// <summary>
        /// Handles different wall run states
        /// </summary>
        private void StateMachine()
        {
            keyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            keyInput = keyInput.normalized;

            //state 1 - Wall running
            if ((wallLeft || wallRight) && keyInput.y > 0 && AboveGround() && !exitingWall)
            {
                if (!pm.isWallRunning)
                    StartWallRun();
                if (!CanWallRunInfinity)
                {
                    if (wallRunTimer > 0)
                        wallRunTimer -= Time.deltaTime;
                    if (wallRunTimer <= 0)
                    {
                        exitingWall = true;
                        exitWallTimer = exitWallTime;
                    }
                }
                if (Input.GetKeyDown(wallJumpKey))
                {
                    WallJump();
                }
            }
            else if (exitingWall)
            {
                if (pm.isWallRunning)
                    StopWallRun();
                if (exitWallTimer > 0)
                    exitWallTimer -= Time.deltaTime;
                if (exitWallTimer <= 0)
                    exitingWall = false;
            }
            else
            {
                if (pm.isWallRunning)
                    StopWallRun();
            }
        }
        /// <summary>
        /// Sets bools,velocity, camera tilt etc.
        /// </summary>
        void StartWallRun()
        {
            pm.isWallRunning = true;
            pm.canDoublJump = false;
            if (!CanWallRunInfinity)
                wallRunTimer = maxWallRunTime;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (wallRight)
            { 
                //camera tilt to left
                leaningInput.SetLeaningInput(-1);
            }
            else
            {
                //camera tilt to right
                leaningInput.SetLeaningInput(1);
            }
          
        }
        /// <summary>
        /// Perform the wall run movement i.e adds force to the player in the wall direction
        /// </summary>
        void WallRunning()
        {
            rb.useGravity = useGravity;
            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
            if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            {
                wallForward = -wallForward;
            }

            
            rb.AddForce(orientation.forward * wallRunForce, ForceMode.Force);

            //push to wall force
            if (!(wallLeft && keyInput.x > 0) && !(wallRight && keyInput.x < 0))
            {
                rb.AddForce(-wallNormal * 100, ForceMode.Force);
            }
        }
        /// <summary>
        /// Resets all bools,gravity etc.
        /// </summary>
        void StopWallRun()
        {
            pm.isWallRunning = false;
            rb.useGravity = true;
            leaningInput.SetLeaningInput(0);
            DoTilt(0);
            
        }
        /// <summary>
        /// Performs wall jumps
        /// </summary>
        void WallJump()
        {
            Debug.Log("wallJump");
            exitingWall = true;
            pm.canDoublJump = true;
            exitWallTimer = exitWallTime;
            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

            Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

            rb.velocity = Vector3.zero;
            rb.AddForce(forceToApply, ForceMode.Impulse);
        }
        /// <summary>
        /// Rotates the camera in the forward axiz by zTili in 0.25s.
        /// </summary>
        /// <param name="zTilt"></param>
        void DoTilt(float zTilt)
        {
            cam.transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        }
        #endregion
    }
}

