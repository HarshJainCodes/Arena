using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{

    [SerializeField] Transform _startLocation;
    [SerializeField] Transform _endLocation;
    [SerializeField] Transform _Control;
    Vector3 _dir;
    [SerializeField] float _upthrustmultiplier=10f;
    [SerializeField] float _directionalThrust = 2f;
    [SerializeField] float time = 3f;
    float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        _dir = (_endLocation.position-_startLocation.position)/Vector3.Magnitude(_endLocation.position - _startLocation.position);
        _dir.y = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Here");
        JumpMath jm=other.gameObject.AddComponent<JumpMath>();
        jm.Starting=other.transform.position;
        jm.Destination=_endLocation.position;
        jm.Control = _Control.position;
        jm.Timing = time;
        jm.Set = true;
        /*Rigidbody rb=other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb?.AddForce(Vector3.up * Vector3.Magnitude(_endLocation.position - _startLocation.position) * _upthrustmultiplier);
            rb?.AddForce(_directionalThrust * _dir);
            Debug.Log("Added Force");
        }
        else
            Debug.Log("Not in here");*/
    }

    

    // Update is called once per frame
    void Update()
    {

    }
}
