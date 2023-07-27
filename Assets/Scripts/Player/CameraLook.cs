using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraLook : MonoBehaviour
{

    public float x_Sensitivity;
    public float y_Sensitivity;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private Camera cam;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform playerArms;

    [SerializeField]
    private LedgeGrabArms ledgeGrabArms;
    // private Transform camHolder;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
    }
    float xRot;
    float yRot;
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * x_Sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * y_Sensitivity;

        

        if (!ledgeGrabArms.Ledge)
        {
            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, -90, 90);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerArms.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
        }
        else
        {
            xRot -= mouseY;
            yRot += mouseX;
            xRot = Mathf.Clamp(xRot, -20, 20);
            yRot = Mathf.Clamp(yRot, -20, 20);
            transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        }
    }

    public void DoFov(int val)
    {
        cam.GetComponent<Camera>().DOFieldOfView(val, 0.25f);
    }
    public void DoTilt(int val)
    {
        transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, val), 0.5f);
    }

}
