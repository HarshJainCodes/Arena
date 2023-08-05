using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeScript : MonoBehaviour
{
    /// <summary>
    /// The target is the player transform 
    /// </summary>
    [SerializeField] private Transform _target;

    /// <summary>
    /// The _self is the main body of the enemy 
    /// </summary>
    [SerializeField] private Transform _self;

    /// <summary>
    /// Thsi script is required as we need to stop it and start redirecting the enemy to the player when it is about to explode.
    /// </summary>
    [SerializeField] private DestinationSetterScript _destinationSetterScript;
    /// <summary>
    /// Explosion Detect radius is the radius within which the enemy will detect the player and then start chasing him.
    /// </summary>
    [Tooltip("The Explode Radius should always be greater than or equal to the height in the destination setter script")] 
    [SerializeField] private float _explodeDetectRadius=13f;
    /// <summary>
    /// Speed is what defines how fast the enemy will approach te player when the enemy detects the same.
    /// </summary>
    [SerializeField] private float _speed=4f;
    /// <summary>
    /// This bool makes sure that the destination setter script is turned on and off only once per cycle of update to not make it ineffeicent or introduce any problems.
    /// </summary>
    bool _triggeronce = false;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        homing();
    }

    /// <summary>
    /// This function is written to guide the enemy to the player when he is in explosion detect radius.
    /// </summary>
    void homing()
    {
        //Debug.LogError(Vector3.Magnitude(_target.position - _self.position));
        if(Vector3.Magnitude(_target.position-_self.position)<_explodeDetectRadius)
        {
            if (!_triggeronce)
            {
                _destinationSetterScript.enabled = false;
                _triggeronce = true;
            }
            Vector3 _direction= (_target.position - _self.position)/ Vector3.Magnitude(_target.position - _self.position);
            _self.position += _direction * Time.deltaTime * _speed;
        }
        else
        {
            if (_triggeronce)
            {
                _destinationSetterScript.enabled = true;

            }
        }
        //Debug.Log(Vector3.Magnitude(_target.position - _self.position));
        if(Vector3.Magnitude(_target.position - _self.position)<8f)
        {
            StartCoroutine(destruct());
        }
    }

    IEnumerator destruct()
    {
        Destroy(GetComponentInParent<Transform>().transform.gameObject);
        yield return null;
    }
}
