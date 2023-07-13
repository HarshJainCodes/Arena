using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private PlayerCamera cam;

    [Header("KeyBinds")]
    public Key crouchKey;
    public Key sprintKey;
    public Key jumpKey;
    
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float groundDrag;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = false;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    private bool isCrouching= false;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
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
    public enum MovementState
    {
        walking,
        sprinting,
        air,
        crouching,
        sliding,
        wallRunning
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        readyToJump = true;
        startYScale = transform.localScale.y;
    }
    
    private void StateHandler()
    {
        if(isWallRunning)
        {
            state = MovementState.wallRunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        if(isSliding)
        {
            state = MovementState.sliding;
            cam.DoFov(70);
            if(OnSlope() && rb.velocity.y<0.1f)
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
            cam.DoFov(50);
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
           
        }

        //Mode  - Sprinting
        else if (grounded && Keyboard.current[sprintKey].isPressed)
        {
            cam.DoFov(65);
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        //Mode - Walking
        else if (grounded)
        {
            cam.DoFov(60);
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        //Mode - Air
        else
        {
            state = MovementState.air;
        }


        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed)>4f && moveSpeed!=0)
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

        while(time<difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            if(OnSlope())

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
        if(Keyboard.current[jumpKey].isPressed && readyToJump && grounded)
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
            isCrouching = true;
        }
        
        //STOP CROUCH
        if (Keyboard.current[crouchKey].wasReleasedThisFrame)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
        }
    }
    private void Update()
    {
        HandleInput();
        CheckGround();
        SpeedControl();
        StateHandler();
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * keyInput.y + orientation.right * keyInput.x;

        //ON SLOPE
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if(rb.velocity.y<0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //on ground
        if(grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        //in air
        else if (!grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        if(!isWallRunning)
            rb.useGravity = !OnSlope();
    }

    private bool CheckGround()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.2f, whatIsGround); 
        return grounded;
    }

    private void SpeedControl()
    {
        //limiting speed on slopes
        if(OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude>moveSpeed)
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

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        Debug.DrawRay(transform.position, Vector3.down*(playerHeight/2+0.5f), Color.red);
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit,playerHeight/2+0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public bool IsPlayerGrounded()
    {
        return grounded;
    }

}
