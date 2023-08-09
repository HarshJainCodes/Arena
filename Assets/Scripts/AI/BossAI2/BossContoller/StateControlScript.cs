using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControlScript : MonoBehaviour
{
    [SerializeField] BossMain _controlHandle;
    [SerializeField] BossState _state=BossState.None;
    BossState previousState;
    void Start()
    {
        previousState= _state;
    }

    // Update is called once per frame
    void Update()
    {
        if (_state != previousState)
        {
            previousState = _state;
            _controlHandle.changeState(_state);
        }
        previousState = _state;
    }

        
}
