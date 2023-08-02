using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using FMODUnity;
using FMOD.Studio;

namespace Arena
{
    public class PlayerMovement : MonoBehaviour
    {
        //Keybinds
        [Header("KeyBinds")]
        [SerializeField]
        private KeyCode sprintKey;
        [SerializeField]
        private KeyCode jumpKey;
        [SerializeField]
        private KeyCode crouchKey;
        [Header("References")]
        [SerializeField]
        
        //Reference to the orientation i.e the players forward direction.
        private Transform orientation;

        //Ref to the script that performs camera movement
        [SerializeField]
        private CameraLook cam;

        //Movement variables
        [Header("Movement")]
        [SerializeField]
        private float gravityScale = 5f;
        private float _MoveSpeed;//the actuall variable that controlls the spped of player, it is set to respective speed of the players current state 
        [SerializeField]
        private float speedAiming = 5f;
        [SerializeField]
        private float speedWalking = 7.0f;
        [SerializeField]
        private float speedSprinting = 10.0f;
        [SerializeField]
        private float speedSliding = 20.0f;
        [SerializeField]
        private float speedWallRunning = 30.0f;
        [SerializeField]
        private float speedClimbing = 20f;
        [SerializeField]
        private float groundDrag = 4f;
        [SerializeField]
        private bool canCrouch = true;
        [SerializeField]
        private bool canJumpWhileCrouch = false;
        [SerializeField]
        private bool canCrouchWhileFalling = false;

        [Range(0.0f, 1.0f)]
        private float walkingMultiplierForward = 1.0f;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float walkingMultiplierSideways = 1.0f;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float walkingMultiplierBackwards = 1.0f;

        private float desiredMoveSpeed;
        private float lastDesiredMoveSpeed;
        [SerializeField]
        private float speedIncreaseMultiplier;
        [SerializeField]
        private float slopeIncreaseMultiplier;

        [Header("Ground Check")]
        [SerializeField]
        private float playerHeight = 2.0f;
        [SerializeField]
        private LayerMask whatIsGround;


        [Header("Jumping")]
        [SerializeField]
        private float jumpForce = 10.0f;

        [SerializeField]
        private float jumpHeight = 10f;
        [SerializeField]
        private float jumpCooldown = 0.25f;
        [SerializeField]
        private float airMultiplier = 0.4f;
        bool readyToJump = false;

        [Header("Crouching")]
        [SerializeField]
        private float crouchSpeed = 3.5f;
        [SerializeField]
        private float crouchYScale = 0.5f;
        private float startYScale;

        [Header("Slope Handling")]
        [SerializeField]
        public float maxSlopeAngle = 50f;
        private RaycastHit slopeHit;
        private bool exitingSlope;


        private Vector2 keyInput;

        Vector3 moveDirection;

        Rigidbody rb;

        private MovementState state;

        [HideInInspector]
        public bool isSliding;
        [HideInInspector]
        public bool isWallRunning;
        [HideInInspector]
        public bool isSprinting;


        [SerializeField]
        private AnimationParameters playerAnim;
        private Weapon equippedWeapon;
        private float standingHeight;
        private Vector3 velocity;

        
        public bool grounded = false;
        public bool wasGrounded = false;
        private bool jumping;
        private float lastJumpTime;
        private bool crouching = false;
        private float landTime;
        [HideInInspector]
        public bool canDoublJump = false;
        public AudioClip jumpStart;
        public AudioClip jumpLand;
        public bool Ledgegrab;

        //Creating audio Instance
        private EventInstance walkSFX;
        private EventInstance runSFX;
        private EventInstance wallrunSFX;





        //all the states the player could have
        public enum MovementState
        {
            walking,
            sprinting,
            air,
            crouching,
            sliding,
            wallRunning,
            aiming,
            climbing,
            idle
        }

        #region UNITY FUNCTIONS
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            readyToJump = true;
            startYScale = transform.localScale.y;

            //setting audio instance
            walkSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.Walk);
            runSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.Run);
            wallrunSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.WallRun);

        }


        private void Update()
        {
            //gets the weapon the player has
            equippedWeapon = playerAnim.GetInventory().GetEquipped();
            wasGrounded = grounded;

            grounded = CheckGround();
            if (grounded && !wasGrounded)
            {
                lastJumpTime = 0.0f;
                jumping = false;
                canDoublJump = false;
                landTime = Time.time;
                AudioManagerServices.instance.PlayOneShot(jumpLand, new AudioSettings(0.5f, 0.0f, true));
                AudioManager.instance.PlayOneShot(FMODEvents.instance.Land, this.transform.position);
            }
            else if (wasGrounded && !grounded)
            {
                lastJumpTime = Time.time;
            }
               
            SpeedControl();
            StateHandler();
            
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

        }


        private void FixedUpdate()
        {
            if(!Ledgegrab)
                MovePlayer();
            //Handles the extra gravity on top of the default rigidbody gravity, increase the gravityScale to shorten the air time
            if(state!= MovementState.wallRunning && rb.useGravity)
            {
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

            }
        }
        #endregion

        #region CUSTOM FUNCTIONS
        /// <summary>
        /// Called in Update, Checks for changes in player state and update the spped accordingly
        /// </summary>
        private void StateHandler()
        {
            //state = wallrunning
            if (isWallRunning)
            {
                state = MovementState.wallRunning;
                desiredMoveSpeed = speedWalking;
                isSprinting = false;
                UpdateSound();
            }
            //state = sliding
            else if (isSliding)
            {
                state = MovementState.sliding;
                //cam.DoFov(70);
                if (OnSlope() && rb.velocity.y < 0.1f)
                {
                    desiredMoveSpeed = speedSliding;
                }
                else
                {
                    desiredMoveSpeed = speedSprinting;
                }
                UpdateSound();

            }
            //state = crouching
            else if (playerAnim.IsCrouching())
            {
                //cam.DoFov(50);
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
                UpdateSound();

            }
            //state = Sprinting
            else if (grounded && playerAnim.IsRunning())
            {
                //cam.DoFov(65);
                isSprinting = true;
                state = MovementState.sprinting;
                desiredMoveSpeed = speedSprinting;
                UpdateSound();

            }
            //state = Walking && aiming
            else if (grounded && playerAnim.IsAiming())
            {
                state = MovementState.aiming;
                desiredMoveSpeed = speedAiming;
                UpdateSound();
            }
            //state = walking
            else if (grounded && (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                // cam.DoFov(60);
                state = MovementState.walking;
                desiredMoveSpeed = speedWalking;
                isSprinting = false;
                UpdateSound();

            }
            else if(grounded && !(Input.anyKey))
            {
                state = MovementState.idle;
                isSprinting = false;
                UpdateSound();
            }
            //state = Air
            else
            {
                state = MovementState.air;
                isSprinting = false;
                UpdateSound();
            }

            //Gradually decreases the speed from lastDesiredMoveSpeed to desiredMoveSpped if the difference is greater than 4
            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && _MoveSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
                
            }
            else
            {
                _MoveSpeed = desiredMoveSpeed;
               

            }
            lastDesiredMoveSpeed = desiredMoveSpeed;
        }

        /// <summary>
        /// Decreases the spped over time, so to maintain momentum
        /// </summary>
        /// <returns></returns>
        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - _MoveSpeed);
            float startValue = _MoveSpeed;

            while (time < difference)
            {
                _MoveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
                if (OnSlope())

                {
                    float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                    time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
                }
                else
                {
                    time += Time.deltaTime;
                }
                yield return null;
            }
            _MoveSpeed = desiredMoveSpeed;
        }
        private void MovePlayer()
        {
            keyInput = playerAnim.GetInputMovement();
            keyInput = keyInput.normalized;
            //calculate movement direction
            moveDirection = orientation.forward * keyInput.y + orientation.right * keyInput.x;

            //ON SLOPE
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * _MoveSpeed * 20f, ForceMode.Force);
                if (rb.velocity.y < 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }

            //on ground
            if (grounded)
            {

                rb.AddForce(moveDirection * _MoveSpeed * 10f, ForceMode.Acceleration);

            }
            //in air
            else if (!grounded)
                rb.AddForce(moveDirection * _MoveSpeed * 10f * airMultiplier, ForceMode.Acceleration);

            /*if (!isWallRunning)
                rb.useGravity = !OnSlope();*/
        }
        /// <summary>
        /// Checks is player is on ground or not.
        /// </summary>
        /// <returns></returns>
        private bool CheckGround()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f, whatIsGround);
            return grounded;
        }
        /// <summary>
        /// Limits the player speed from exceeding infinitely.
        /// </summary>
        private void SpeedControl()
        {
            //limiting speed on slopes
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > _MoveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * _MoveSpeed;
                }
            }
            //limiting speed on ground and air
            else
            {
                Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                if (flatVelocity.magnitude > _MoveSpeed)
                {
                    Vector3 limitedVelocity = flatVelocity.normalized * _MoveSpeed;
                    rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
                }
            }
        }
        /// <summary>
        /// Jump function called from input events.
        /// </summary>
        public void Jump()
        {

            if (crouching && !canJumpWhileCrouch)
                return;
            if (!grounded && !canDoublJump)
                return;
            if (canDoublJump)
            {
                DoubleJump();
                return;
            }
           // AudioManagerServices.instance.PlayOneShot(jumpStart, new AudioSettings(0.5f, 0.0f, true));
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Jump, this.transform.position);

            canDoublJump = true;
            jumping = true;
            landTime = 0;
            exitingSlope = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            float jf = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));
            rb.AddForce(transform.up * jf, ForceMode.Impulse);
            Debug.Log("jump");
            lastJumpTime = Time.deltaTime;
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);

        }
        /// <summary>
        /// Performs the second jump while in air
        /// </summary>
        public void DoubleJump()
        {

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            float jf = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));

            rb.AddForce(transform.up * jf, ForceMode.Impulse);
            lastJumpTime = Time.deltaTime;
            canDoublJump = false;
        }
        /// <summary>
        /// Reset Jump variables after jump
        /// </summary>
        private void ResetJump()
        {
            readyToJump = true;

            exitingSlope = false;
        }
        /// <summary>
        /// Checks if player is on slopes
        /// </summary>
        /// <returns></returns>
        public bool OnSlope()
        {
            Debug.DrawRay(transform.position, Vector3.down * (playerHeight / 2 + 0.1f), Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }
            return false;
        }
        /// <summary>
        /// Gets the direction in which the force needs to be applied on the slope. Cross product of slope normal and player right direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }

        /// <summary>
        /// Handle the crouch movement
        /// </summary>
        /// <param name="newCrouching"></param>
        public void Crouch(bool newCrouching)
        {
            //Set the new crouching value.
            crouching = newCrouching;
            Debug.Log(crouching);
            //Update the capsule's height.
            transform.localScale = new Vector3(transform.localScale.x, crouching ? crouchYScale : startYScale, transform.localScale.z);
            if (crouching && !isSliding)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                crouching = true;
            }
            else if (!crouching)
            {
                crouching = false;
            }

        }
        /// <summary>
        /// if CanCrouch then crouch else UnCrouch
        /// </summary>
        /// <param name="value"></param>
        public void TryCrouch(bool value)
        {
            if (value && CanCrouch(true))
                Crouch(true);
            else if (!value)
                StartCoroutine(nameof(TryUncrouch));
        }
        private IEnumerator TryUncrouch()
        {
            yield return new WaitUntil(() => CanCrouch(false));
            Crouch(false);
        }
        /// <summary>
        /// Checks if player can Crouch 
        /// </summary>
        /// <param name="value"></param>
        public bool CanCrouch(bool newCrouching)
        {
            //Always block crouching if we need to.
            if (canCrouch == false)
                return false;
            Debug.Log("Pass1");
            //If we're in the air, and we cannot crouch while in the air, then we can ignore this execution!
            if (grounded == false && canCrouchWhileFalling == false)
                return false;
            Debug.Log("Pass2");

            //The controller can always crouch, the issue is un-crouching!
            if (newCrouching)
                return true;
            Debug.Log("Pass3");

            return true;

        }
        #endregion

        #region ALL THE GETTER FUNCTIONS
        public void TryToggleCrouch() => TryCrouch(!crouching);
        public bool IsPlayerGrounded() => grounded;
        public bool IsCrouching() => crouching;
        public float GetLastJumpTime() => lastJumpTime;
        public bool IsJumping() => jumping;
        public bool WasGrounded() => wasGrounded;
        public float GetMultiplierForward() => walkingMultiplierForward;
        public float GetMultiplierSideways() => walkingMultiplierSideways;
        public float GetMultiplierBackwards() => walkingMultiplierBackwards;
        public float GetLandTime() => landTime;
        public Vector3 GetVelocity() => rb.velocity;
        public float GetPlayerHeight() => playerHeight;
        #endregion

        #region ALL THE SETTER FUNCTIONS
        public void SetCrouch(bool val)
        {
            crouching = val;
        }
        #endregion


        /*public void SetClimbing(bool val)
        {
            climbing = val;
        }*/
        //Functionality of all sounds
       private void UpdateSound()
        {
            //state = wallrunning
            if (isWallRunning)
            {
                PLAYBACK_STATE pbState;
                wallrunSFX.getPlaybackState(out pbState);
                if (pbState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    wallrunSFX.start();

                }
            }
            
           
           
            else
            {  
                    wallrunSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    
            }
            if (grounded && playerAnim.IsRunning() && isSprinting == true)
            {
                PLAYBACK_STATE pbState;
                runSFX.getPlaybackState(out pbState);
                if (pbState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    runSFX.start();
                }
            }
            else
            {
                
                runSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
               
            }

            if (state == MovementState.walking)
            {
                PLAYBACK_STATE pbState;
                walkSFX.getPlaybackState(out pbState);
                if (pbState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    walkSFX.start();
                    //Debug.LogError("walk");
                }
            }
            /*  else
              {
                  walkSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
              }*/

            if (state == MovementState.idle || jumping==true)
            {
                //Debug.LogError("idle");
                wallrunSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                runSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                walkSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

        }
    }

}
