using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwivel : MonoBehaviour
{
    /// <summary>
    /// This Float controls the speed at which you can orbit the object
    /// </summary>
    [SerializeField]    private float _TurnSpeed = 5.0f;
    /// <summary>
    /// This GameObject holds the component of what you will be looking at
    /// </summary>
    [SerializeField]    private GameObject _Player;

    /// <summary>
    /// This transform component will store the transform of the swivel point
    /// </summary>
    private Transform _PlayerTransform;
    /// <summary>
    /// This computes and stores the offset from the swivel point
    /// </summary>
    private Vector3 _Offset;

    [SerializeField]    private float _YOffset = 4.0f;
    [SerializeField]    private float _ZOffset = 4.0f;
    [SerializeField]    private float _XOffset = 4.0f;

    void Start()
    {
        _PlayerTransform = _Player.transform;
        _Offset = new Vector3(_PlayerTransform.position.x + _XOffset, _PlayerTransform.position.y + _YOffset, _PlayerTransform.position.z + _ZOffset);
        _Offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * _TurnSpeed, Vector3.up) * _Offset;
        transform.position = _PlayerTransform.position + _Offset;
        transform.LookAt(_PlayerTransform.position);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            // We calculate the offset and turn based on mouse axis
            _Offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * _TurnSpeed*Time.fixedDeltaTime, Vector3.up) * _Offset;
            transform.position = _PlayerTransform.position + _Offset;
            transform.LookAt(_PlayerTransform.position);
        }
    }

    public void ChangeOffset(float increment)
    {
        _XOffset += increment;
        _YOffset += increment;
        _Offset = new Vector3(_PlayerTransform.position.x + _XOffset, _PlayerTransform.position.y + _YOffset, _PlayerTransform.position.z + _ZOffset);
        _Offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * _TurnSpeed, Vector3.up) * _Offset;
    }
}
