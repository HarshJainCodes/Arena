using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using System;
using UnityEngine.InputSystem;
public class NewMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float walkingMultiplierForward = 1.0f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float walkingMultiplierSideways = 1.0f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float walkingMultiplierBackwards = 1.0f;

    [SerializeField]
    private LayerMask WhatIsGround;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    public float airMultiplier;

    [SerializeField]
    public float maxSlopeAngle;

    [SerializeField]
    private Key jumpKey;

    [SerializeField]
    private Key crouchKey;

    [SerializeField]
    private Key sprintKey;

    [SerializeField]
    private float jumpCooldown;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float wallRunSpeed;

    [SerializeField]
    private float slideSpeed;

    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float crouchSpeed;
    
    [SerializeField]
    private float speedIncreaseMultiplier;
    
    [SerializeField]
    private float slopeIncreaseMultiplier;

    [SerializeField]
    private float crouchYScale;

    [SerializeField]
    private bool canCrouch;

    [SerializeField]
    private bool canCrouchWhileFalling;

    private CapsuleCollider playerCollider;
    private Rigidbody rb;
    private CharacterBehaviour playerCharacter;
    private WeaponBehaviour equippedWeapon;
    private float standingHeight;
    private bool isGrounded;
    private bool wasGrounded;
    private bool jumping;
    private bool crouching;
    private float lastJumpTime;

    private Vector3 moveDirection;
    private Vector3 keyInput;
    private bool exitingSlope;
    private RaycastHit slopeHit;
    private bool isWallRunning;
    private bool readyToJump;
    private bool isSliding;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private float moveSpeed;
    private MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air,
        crouching,
        sliding,
        wallRunning
    }
    private void Awake()
    {
        playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.freezeRotation = true;
        standingHeight = playerCollider.height;
        readyToJump = true;

    }

    private void Update()
    {
        equippedWeapon = playerCharacter.GetInventory().GetEquipped();
        isGrounded = Grounded();
        if (isGrounded && !wasGrounded)
        {
            jumping = false;
            lastJumpTime = 0.0f;
        }
        else if (wasGrounded && !isGrounded)
            lastJumpTime = Time.time;

        HandleInput();
        SpeedControl();
        StateHandler();
        wasGrounded = isGrounded;
    }
    private void StateHandler()
    {
        if (isWallRunning)
        {
            state = MovementState.wallRunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        if (isSliding)
        {
            state = MovementState.sliding;
            //cam.DoFov(70);
            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        else if (Keyboard.current[crouchKey].isPressed)
        {
            //cam.DoFov(50);
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;

        }

        //Mode  - Sprinting
        else if (isGrounded && Keyboard.current[sprintKey].isPressed)
        {
            //cam.DoFov(65);
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        //Mode - Walking
        else if (isGrounded)
        {
            //cam.DoFov(60);
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        //Mode - Air
        else
        {
            state = MovementState.air;
        }


        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
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
        moveSpeed = desiredMoveSpeed;
    }
    void HandleInput()
    {
        keyInput = GameInput.instance.GetMovementVectorNormalized();

        //when to JUMP
        if (Keyboard.current[jumpKey].isPressed && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //START CROUCH
        if (Keyboard.current[crouchKey].wasPressedThisFrame)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
        }

        //STOP CROUCH
        if (Keyboard.current[crouchKey].wasReleasedThisFrame)
        {
            transform.localScale = new Vector3(transform.localScale.x, standingHeight, transform.localScale.z);
            crouching = false;
        }
    }

    private void SpeedControl()
    {
        //limiting speed on slopes
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //limiting speed on ground and air
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }
    private void MoveCharacter()
    {
        moveDirection = transform.forward * keyInput.y + transform.right * keyInput.x;

        //ON SLOPE
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //on ground
        if (isGrounded)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        //in air
        else if (!isGrounded)
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        if (!isWallRunning)
            rb.useGravity = !OnSlope();
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private bool OnSlope()
    {
        Debug.DrawRay(transform.position, Vector3.down * (standingHeight / 2 + 0.5f), Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, standingHeight/ 2 + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private bool Grounded()
    {
       var grounded = Physics.Raycast(transform.position, Vector3.down, standingHeight / 2 + 0.2f, WhatIsGround);
        return grounded;
    }

    public bool WasGrounded() => wasGrounded;

    public bool IsJumping() => jumping;

    public bool CanCrouch(bool newCrouching)
    {
        if (isGrounded)
        {
            return true;
        }

        if (newCrouching)
        {
            return true;
        }

        return false;
    }

    public bool IsCrouching() => crouching;

    public float GetLastJumpTime() => lastJumpTime;
    public float GetMultiplierForward() => walkingMultiplierForward;
    public float GetMultiplierSideways() => walkingMultiplierSideways;
    public float GetMultiplierBackwards() => walkingMultiplierBackwards;

    public Vector3 GetVelocity() => rb.velocity;
    public  bool IsGrounded() => isGrounded;



}
