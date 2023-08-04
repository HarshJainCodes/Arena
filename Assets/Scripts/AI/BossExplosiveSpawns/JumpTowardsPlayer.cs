using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTowardsPlayer : MonoBehaviour
{
    AIDestinationSetter _setter;
    Transform _player;
    [SerializeField]CharacterController _selfrb;
    float _timer = 0f;
    float _timerMax = 5f;
    // Start is called before the first frame update
    void Start()
    {
        _setter=GetComponent<AIDestinationSetter>();
        _player = _setter.target;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(Mathf.Abs(_player.position.y-transform.position.y)>3 && _timer>_timerMax)
        {
            _timerMax = Random.Range(2f,6f);
            _timer = 0f;
            _selfrb.SimpleMove(((/*(_player.position - transform.position) / Vector3.Magnitude(_player.position - transform.position))*//*+*/transform.up) * 1000f));
        }
    }
}
