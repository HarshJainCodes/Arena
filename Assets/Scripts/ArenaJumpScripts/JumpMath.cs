using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using UnityEditor.ShaderGraph;
using UnityEngine;

public class JumpMath : MonoBehaviour
{
    Vector3 _destination;
    Vector3 _start;
    Vector3 _control;
    float _timer = 3f;
    float _time=0f;
    public bool Set = false;
    public Vector3 Destination { set { _destination = value; } }
    public Vector3 Starting { set { _start = value; } }
    public Vector3 Control { set { _control = value; } }
    
    public float Timing { set { _timer = value; } }

    // Update is called once per frame
    void Update()
    {
        if(Set)
        {
            _time += Time.deltaTime/_timer;
            transform.position = evaluate(_time);
            if(_time>=1)
            {
                Destroy(gameObject.GetComponent<JumpMath>());
            }
        }
    }

    public Vector3 evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(_start, _control, t);
        Vector3 cb = Vector3.Lerp(_control, _destination, t);
        return Vector3.Lerp(ac, cb, t);
    }
}
