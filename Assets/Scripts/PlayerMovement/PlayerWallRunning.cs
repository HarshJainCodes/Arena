using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerWallRunning : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    private Playermovement pm;
    private Rigidbody rb;

    [Header("Parameters")]
    public bool CanWallRunInfinity = false;
    public float maxWallRunTime;
    private float wallRunTimer;
    public bool useGravity;

    [Header("Keybinds")]
    public Key wallJumpKey;

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
    private PlayerCamera cam;
    private void Start()
    {
        pm = GetComponent<Playermovement>();
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
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        keyInput = GameInput.instance.GetMovementVectorNormalized();

       //state 1 - Wall running
        if((wallLeft||wallRight) && keyInput.y>0 && AboveGround()&&!exitingWall)
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
            if(Keyboard.current[wallJumpKey].wasPressedThisFrame)
            {
                WallJump();
            }
        }
        else if(exitingWall)
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

    void StartWallRun()
    {
        pm.isWallRunning = true;
        if(!CanWallRunInfinity)
            wallRunTimer = maxWallRunTime;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        cam.DoFov(90);
        if (wallLeft)
            cam.DoTilt(-5f);
        if(wallRight)
            cam.DoTilt(5f);
    }
    void WallRunning()
    {
        rb.useGravity = useGravity;
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        if((orientation.forward-wallForward).magnitude>(orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
       
        //float dot = Vector3.Dot(cam.GetComponent<Transform>().forward, orientation.forward);
        rb.AddForce(cam.GetComponent<Transform>().forward* wallRunForce, ForceMode.Force);

        //push to wall force
        if(!(wallLeft && keyInput.x>0)&&!(wallRight &&keyInput.x<0))
        { 
        rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
    }
    void StopWallRun()
    {
        pm.isWallRunning = false;
        rb.useGravity = true;
        cam.DoFov(60);
        cam.DoTilt(0);
    }

    void WallJump()
    {
        exitingWall = true;

        exitWallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallHit.normal :leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
