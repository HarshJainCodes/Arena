using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool _enable = true;
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;

    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform cameraHolder = null;
    private Playermovement pm;
    private Rigidbody rb;
    private float toggelSpeed = 3f;
    private Vector3 startPos;

    private void Awake()
    {
        startPos = camera.localPosition;
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Playermovement>();
    }
    private void Update()
    {
        if (!_enable) return;
        CheckMotion();
        ResetPosition();
        //camera.LookAt(FocusTarget());
    }
    private void CheckMotion()
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        if (speed < toggelSpeed) return;
        if (!pm.IsPlayerGrounded()) return;
        PlayMotion(FootStepMotion());
    }
    private void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion;
    }
    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time*frequency/2)*amplitude*2;
        return pos;
    }
    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.z, transform.position.z);
        pos += cameraHolder.forward * 15f;
        return pos;
    }
}
