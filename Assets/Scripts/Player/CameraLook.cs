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

    // private Transform camHolder;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * x_Sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * y_Sensitivity;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90, 90);
        playerArms.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(int val)
    {
        cam.GetComponent<Camera>().DOFieldOfView(val, 0.25f);
    }
}
