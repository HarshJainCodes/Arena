using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderCrawlScript : MonoBehaviour
{
    /// <summary>
    /// Gravity of the overground pathfinding
    /// </summary>
    [SerializeField] float _gravity = 9.8f;
    /// <summary>
    /// Handle for the current gameobjects Characrter Controller
    /// </summary>
    [SerializeField] CharacterController _self;
    /// <summary>
    /// Holds the players transform
    /// </summary>
    [SerializeField] Transform _target;
    /// <summary>
    /// Speed of the gameobject
    /// </summary>
    [SerializeField] float _speed=10f;
    /// <summary>
    /// How fast the gamobjects climbs up obstacles
    /// </summary>
    [SerializeField] float _crawlUpSpeed;
    /// <summary>
    /// Checks if gravity is supposed to be on during climb
    /// </summary>
    [SerializeField] bool _gravityIsEnabled = true;
    /// <summary>
    /// Vector to hold the direction in which the player tranform is
    /// </summary>
    Vector3 _direction;
    /// <summary>
    /// The obstacle layer it needs to climb
    /// </summary>
    [SerializeField] LayerMask _wallLayer;
    /// <summary>
    /// The minimum distance from the player the gameobject should stop moving towards it
    /// </summary>
    [SerializeField] float _stopDistance=2f;
    /// <summary>
    /// This is a handle to the AI path script in gameobject to allow it to enable it and disable it.
    /// </summary>
    [SerializeField] AIPath _aiPathfinding;
    /// <summary>
    /// The height of the first floor acting as the threshold to determin whether to use this script or AI path finding based on Astar.
    /// </summary>
    [SerializeField] float _firstFloorHeight;
    /// <summary>
    /// This bool decides whether to use A* or direct pathfinding
    /// </summary>
    bool _isDirectPathfinding = true;
    /// <summary>
    /// It contains the mainmodel of the gameObject
    /// </summary>
    [SerializeField] Transform _mainModel;
    /// <summary>
    /// animator controller for minions
    /// </summary>
    [SerializeField] Animator _gameObjectAnimator;

    [SerializeField] GameObject _trails;
    private void Start()
    {
        _target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (_isDirectPathfinding)
        {
            if (_gravityIsEnabled)
            {
                gravity();
                setRotation();
            }
            if(!_gameObjectAnimator.GetBool("Explode"))
            _moveToTarget();
            checkWall();
            crawlUp();
            
        }
        switchPathfindings();
        turnOffTrails();
    }

    /// <summary>
    /// This funciton takes care of applying gravity to the character controller of current gameObject.
    /// </summary>
    void gravity()
    {
        _self?.Move(new Vector3(0,-_gravity*Time.deltaTime, 0));
    }

    /// <summary>
    /// This function moves the character controller in the direction of the 
    /// <see cref="_target"/> and does not operate on the Y-axis
    /// </summary>
    void _moveToTarget()
    {
        _direction = (_target.position - transform.position)/Vector3.Magnitude((_target.position - transform.position));
        _direction.y = 0;
        if(Vector3.Magnitude((_target.position - transform.position))>_stopDistance)
        _self?.Move(_direction*_speed*Time.deltaTime);
    }

    //Test code to check if raycast was working
/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position - new Vector3(0, transform.localScale.y/2, 0), _direction); 
    }*/

    /// <summary>
    /// This function uses physics.raycast to detect wall in the direction of the <see cref="_target"/> transform and enables and disables gravity accordingly
    /// </summary>
    void checkWall()
    {
        //Raycast(transform.position, _direction, 2, _wallLayer);
        if (Physics.Raycast(transform.position-new Vector3(0,transform.localScale.y/2,0),_direction,4,_wallLayer))
        {
            _gravityIsEnabled= false;
        }
        else
        {
            _gravityIsEnabled= true;
        }
    }

    /// <summary>
    /// this function allows the gameObject to crawl up walls and flat structures to reach the destination
    /// </summary>
    void crawlUp()
    {
        if(!_gravityIsEnabled)
        {
            _self?.Move(new Vector3(0, _crawlUpSpeed*Time.deltaTime, 0));
        }
    }

    void setRotation()
    {
        Vector3 temp = new Vector3(_target.position.x, 0f, _target.position.z);
        _mainModel.LookAt(temp);
        _mainModel.localEulerAngles = new Vector3(0f,_mainModel.localEulerAngles.y,0f);
    }
    /// <summary>
    /// This function allows the gameObject to switch between A* pathfinding and this script if A* pathfinding exists on the scripts.
    /// </summary>
    void switchPathfindings()
    {
        if (_aiPathfinding != null)
        {
            if (_target.transform.position.y < _firstFloorHeight || Vector3.Magnitude(_target.position-transform.position)>100)
            {
                _aiPathfinding.enabled = true;
                _isDirectPathfinding = false;
            }
            else
            {
                _aiPathfinding.enabled = false;
                _isDirectPathfinding = true;
            }
        }
    }

    void turnOffTrails()
    {
        if(_self.isGrounded)
        {
            _trails.SetActive(false);
        }
    }
}
