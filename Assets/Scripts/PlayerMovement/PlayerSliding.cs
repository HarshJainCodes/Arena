using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSliding : MonoBehaviour
{
    [Header("KeyBinds")]
    public Key slideKey;

    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private Playermovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYscale;
    private float startYScale;

    //private bool sliding = false;
    private Vector2 keyInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Playermovement>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        keyInput = GameInput.instance.GetMovementVectorNormalized();
        if(Keyboard.current[slideKey].wasPressedThisFrame &&(keyInput.x!=0||keyInput.y!=0) && pm.IsPlayerGrounded())
        {
            StartSlide();
        }
        
        if(Keyboard.current[slideKey].wasReleasedThisFrame && pm.isSliding)
        {
            StopSlide();
        }

    }
    private void FixedUpdate()
    {
        if (pm.isSliding)
            SlidingMovement();
    }
    private void StartSlide()
    {
        pm.isSliding = true;

        transform.localScale = new Vector3(transform.localScale.x, slideYscale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        slideTimer = maxSlideTime;
    }
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * keyInput.y + orientation.right * keyInput.x;

        //sliding normal i.e on flat planes and not on slopes
        if(!pm.OnSlope() || rb.velocity.y>-0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        //slide down the slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }
        if (slideTimer <= 0)
            StopSlide();
    }
    private void StopSlide()
    {
        pm.isSliding = false;   
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        //rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }
}
