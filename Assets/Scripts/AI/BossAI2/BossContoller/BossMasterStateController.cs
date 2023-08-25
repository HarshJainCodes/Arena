using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMasterStateController : MonoBehaviour
{
    [SerializeField] BossMain _boss;
    [SerializeField] SpawnManager _spawnManager;
    [SerializeField] Transform _player;
    [SerializeField] Transform _bossSpawns;
    [SerializeField] float duration = 10f;
    [SerializeField] AIPath _aiPath;
    [SerializeField] BossHealth _health;

    float timer = 0;
    bool initialSpawnWaves = true;
    bool next = false;
    bool triggered = true;
    [SerializeField] bool chaseSetter = false;
    // Start is called before the first frame update
    void Start()
    {
        _boss.changeState(BossState.Observe);
        //StartCoroutine(StartTime());
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!initialSpawnWaves && !_boss._animationController.GetBool("SpawnEnemies") && _bossSpawns.childCount<20 && !triggered)
        {
            triggered = true;
            _boss.changeState(BossState.Spawn);
            //jumpOnce = true;
            next = true;
        }
        if (initialSpawnWaves && _spawnManager.CurrentWave==1 && _spawnManager.Enemies.Count < 20)//_spawnManager.NumberOfWaves && _spawnManager.Enemies.Count<20)
        {
            _boss.changeState(BossState.Spawn);
            initialSpawnWaves=false;
        }*/


        if (_boss.waveNo == 0 && _spawnManager.CurrentWave==1 && initialSpawnWaves)
        {
            _boss.changeState(BossState.Start);
            initialSpawnWaves = false;
        }

        if (_boss.waveNo == 1 && triggered)
        {
            _boss.changeState(BossState.Spawn);
            next = true;
            triggered = false;
        }
        if (next)
        {
            timer += Time.deltaTime;
            if(timer>duration)
            {
                _health.IsInvulnerable = false;
                _boss.changeState(BossState.Jump);
                next = false;
                timer = 0;
            }
        }
        if(_boss._stateMachine.CurrentState==BossState.Jump || _boss._stateMachine.CurrentState ==BossState.Attack)
        {
            chaseSetter = true;
        }
        if(_boss._stateMachine.CurrentState==BossState.None && chaseSetter)
        {
            _boss.changeState(BossState.Chase);
            chaseSetter=false;
        }

        if(_boss._stateMachine.CurrentState==BossState.Chase )
        {
            int rng=Random.Range(0, 1000);
            if(rng<100 && Vector3.Magnitude(_player.position-_boss.transform.position)>40 && Mathf.Abs(_player.position.y-transform.position.y)>30f)
            {
               // Debug.Log("Dashing");
                _boss.changeState(BossState.Jump);
            }
            else if(rng<40 && Vector3.Magnitude(_player.position - _boss.transform.position) >80 )
            {
                _boss.changeState(BossState.Dash);
            }
            
        }
    }
    /*IEnumerator StartTime()
    {
        yield return new WaitForSeconds(5f);
        initialSpawnWaves = true;
        yield return null;
    }*/
}
