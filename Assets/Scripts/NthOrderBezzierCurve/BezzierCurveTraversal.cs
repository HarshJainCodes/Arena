using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BezzierCurveTraversal : MonoBehaviour
{
    [SerializeField] Transform[] _transforms;
    [SerializeField] float _completionTime;
    public AnimationCurve animationCurve;
    Vector3[] _positions;
    int count = 0;
    float timer = 0;
    [SerializeField]bool run = false;

    Vector3 _positionOfObject;
    // Start is called before the first frame update
    void Start()
    {
        _positions = new Vector3[_transforms.Length];
        for(int i=0;i<_transforms.Length;i++)
        {
            _positions[i] = _transforms[i].position;
        }
        count=_transforms.Length;
    }

    public void setPoints(Transform[] Ts)
    {
        _transforms = new Transform[Ts.Length];
        _transforms = Ts;
    }

    public void setBool()
    {
        run = true;
    }

    Vector3[] compute(Vector3[] pos,float step)
    {
        Vector3[] temp = new Vector3[pos.Length-1];
        for(int i=0;i<pos.Length-1;i++)
        {
            temp[i] = Vector3.Lerp(pos[i], pos[i + 1], step);
        }
        count--;
        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            _positionOfObject = outputBezzierCurve();
            transform.position = _positionOfObject;
            set();
        }
    }

    Vector3 outputBezzierCurve()
    {
        timer += Time.deltaTime / _completionTime;//(_completionTime-animationCurve.Evaluate(timer)*_completionTime);
        while (count != 1 && timer <= 1)
        {
            _positions = compute(_positions, timer);
        }
        if (timer >=1)
        {
            Destroy(gameObject.GetComponent<BezzierCurveTraversal>());
        }
        return _positions[0];
    }

    private void set()
    {
        _positions = new Vector3[_transforms.Length];
        for (int i = 0; i < _transforms.Length; i++)
        {
            _positions[i] = _transforms[i].position;
        }
        count = _transforms.Length;
    }

    private void OnDestroy()
    {
        transform.position = _transforms[_transforms.Length - 1].position;
    }

    /*private void OnDrawGizmos()
    {
        for(int i=0;i<20;i++)
        {

        }
    }*/
}
