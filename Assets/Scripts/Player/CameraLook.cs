using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraLook : MonoBehaviour
{
    [Header("Sensitivity variables")]
    [SerializeField]
    private float x_Sensitivity;
    [SerializeField]
    private float y_Sensitivity;

    [Header("References")]
    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform playerArms;//ref to player arms root object
    [SerializeField]
    private LedgeGrabArms ledgeGrabArms;//ref to the script that handles ledge grabbing.

    private Camera cam;

    //Variables that store rotation input and applies to camera and other objects while not ledge grabbing
    float xRotation;
    float yRotation;

    //Variables that store rotation input and applies to camera and other objects while ledge grabbing
    float xRot;
    float yRot;

    #region UNITY FUNCTIONS
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
    }
    
    private void Update()
    {
        //inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * x_Sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * y_Sensitivity;

        //if not ledge grabbing
        if (!ledgeGrabArms.Ledge)
        {
            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, -90, 90);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerArms.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
        }
        //if ledge grabbing
        else
        {
            xRot -= mouseY;
            yRot += mouseX;
            xRot = Mathf.Clamp(xRot, -20, 20);
            yRot = Mathf.Clamp(yRot, -20, 20);
            transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        }
    }
    #endregion

    #region CUSTOM FUNCTIONS
    /// <summary>
    /// Performs camera FOV shift, lerping between current and "val".
    /// </summary>
    /// <param name="val"></param>
    public void DoFov(int val)
    {
        cam.GetComponent<Camera>().DOFieldOfView(val, 0.25f);
    }
    /// <summary>
    /// Performs camera tilts, lerping between current and "val".
    /// </summary>
    /// <param name="val"></param>
    public void DoTilt(int val)
    {
        transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, val), 0.5f);
    }
    #endregion
}
