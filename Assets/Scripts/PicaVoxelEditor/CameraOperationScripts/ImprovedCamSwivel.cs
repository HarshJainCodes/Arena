using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

/// <summary>
/// This script allows camera to swivel around the object that we are trying to edit
/// </summary>
public class ImprovedCamSwivel : MonoBehaviour
{
    /// <summary>
    /// This variable determines wherther we can swivel around around the camera or not
    /// </summary>
    [SerializeField] private bool _CanMove = false;

    /// <summary>
    /// The Actual object that we want to swivel about.
    /// </summary>


    [SerializeField] private Transform _SwivelObject;

    /// <summary>
    /// Radius for the radial coordinate system
    /// </summary>
    [SerializeField] private float _Radius=3f;
    /// <summary>
    /// Theta of the radial coordinate system
    /// </summary>
    [SerializeField] private float _Theta=45f;
    /// <summary>
    /// Phi of the radial coordinate system
    /// </summary>
    [SerializeField] private float _Phi=45f;
    /// <summary>
    /// Float to determin how fast we can move the camera
    /// </summary>
    [SerializeField] private float _SwivelSpeed = 0f;

    private Vector3 _PositionCastesian;
    // Start is called before the first frame update
    void Start()
    {
        //Calculations to convert radial to cartesian as the coordinate system that unity uses is cartesian
        float x = _Radius * Mathf.Cos(_Theta) * Mathf.Sin(_Phi);
        float y = _Radius * Mathf.Sin(_Theta) * Mathf.Sin(_Phi);
        float z = _Radius * Mathf.Cos(_Phi);
        _PositionCastesian = new Vector3(x, y, z);
        //initially setting the transform of the system.
        transform.position = _PositionCastesian;
    }

    // Update is called once per frame
    void Update()
    {
        //radial to cartesian conversion
        _InputScript();
        float x = _Radius * Mathf.Cos(_Theta) * Mathf.Sin(_Phi);
        float z = _Radius * Mathf.Sin(_Theta) * Mathf.Sin(_Phi);
        float y = _Radius * Mathf.Cos(_Phi);
        _PositionCastesian = new Vector3(x, y, z);
        transform.position = _PositionCastesian;

        //To have the camera constantly point to the swivel transform
        transform.LookAt(_SwivelObject);
    }

    /// <summary>
    /// This method is used to swivel around the object that we are editing as well as clamp the movement of said camera 
    /// </summary>
    private void _InputScript()
    {
        if (Input.GetMouseButton(1))
        {
            _Phi = Mathf.Clamp((_Phi + Input.GetAxis("Mouse Y") * Time.deltaTime * _SwivelSpeed), Mathf.PI/6, 5*Mathf.PI/6);
            _Theta = Mathf.Clamp((_Theta - Input.GetAxis("Mouse X") * Time.deltaTime * _SwivelSpeed), -Mathf.PI, Mathf.PI);
        }
        _Radius = _Radius - Input.mouseScrollDelta.y;
    }
    /// <summary>
    /// Getter and setter method for changing the
    /// <see cref="_CanMove"/>
    /// bool 
    /// </summary>
    public bool CanMove { get { return _CanMove; } set { _CanMove = value; } }

    /// <summary>
    /// Setter method to change the swivel object if necessary.
    /// </summary>
    public Transform ChangeTransform { set { _SwivelObject = value; } }
}
