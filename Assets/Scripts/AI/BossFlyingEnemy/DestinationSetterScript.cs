using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSetterScript : MonoBehaviour
{
    /// <summary>
    /// The target is the player
    /// </summary>
    public Transform _target;
    //[SerializeField] CharacterController _charContoller;
    /// <summary>
    /// Parent refers to the parent of current gameObject
    /// </summary>
    [SerializeField] Transform _parent;
    /// <summary>
    /// movespeed determines the speed of the enemy when he is approaching the player.
    /// </summary>
    [SerializeField] float _moveSpeed = 5f;
    /// <summary>
    /// Height determined the height at which the enemy floats above the player.
    /// </summary>
    [SerializeField] float _height = 8f;
    /// <summary>
    /// Main Model is the actual model that turns.
    /// </summary>
    [SerializeField] Transform _mainModel;
    /// <summary>
    /// Forbidden height that you cannot actually go down below
    /// </summary>
    [SerializeField] float _minHeight=105;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        moveSelf();
    }
    public void moveSelf()
    {
        Vector3 _direction=(_target.position-_parent.position)/Vector3.Magnitude(_target.position - _parent.position);
        _direction.y = 0;
        Vector3 _yDirection=new Vector3(0,0,0);
        if (Mathf.Abs(Mathf.Max(_target.position.y + _height, _minHeight) - _parent.position.y) > 1)
        {
            _yDirection = new Vector3(0, (Mathf.Max(_target.position.y + _height,_minHeight) - _parent.position.y) / Mathf.Abs(Mathf.Max(_target.position.y + _height, _minHeight) - _parent.position.y), 0);
        }
        else
        {
            _yDirection = new Vector3(0,0,0);
        }
        _parent.position = _parent.position + (_direction * _moveSpeed*Time.deltaTime) +(_yDirection * _moveSpeed*Time.deltaTime);
        //_charContoller.SimpleMove(_direction*_moveSpeed);
        _mainModel.LookAt(_target);
    }
}
