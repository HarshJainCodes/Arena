using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput instance;
    private PlayerInput playerInput;
    [HideInInspector]
    public bool isCrouching;
    [HideInInspector]
    public bool isSprinting;
    [HideInInspector]
    public bool isSliding;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerInput = new PlayerInput();
        playerInput.GameControls.Movement.Enable();
        playerInput.GameControls.Jump.Enable();
        playerInput.GameControls.Sprint.Enable();
        playerInput.GameControls.Crouch.Enable();
        playerInput.GameControls.Slide.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInput.GameControls.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;   
    }

    
    
}
