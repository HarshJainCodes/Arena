using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMasterStateController : MonoBehaviour
{
    [SerializeField] BossMain _boss;
    [SerializeField] SpawnManager _spawnManager;
    [SerializeField] SpawnMinions _spawnMinionManager;
    [SerializeField] Transform _player;
    [SerializeField] Transform _bossSpawns;
    [SerializeField] float duration = 10f;

    float timer = 0;
    bool initialSpawnWaves = true;
    bool next = false;
    bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        _boss.changeState(BossState.Observe);
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialSpawnWaves && !_boss._animationController.GetBool("SpawnEnemies") && _bossSpawns.childCount==0 && !triggered)
        {
            triggered = true;
            _boss.changeState(BossState.Spawn);
            //jumpOnce = true;
            next = true;
        }
        if (initialSpawnWaves && _spawnManager.CurrentWave==_spawnManager.NumberOfWaves && _spawnManager.Enemies.Count==0)
        {
            _boss.changeState(BossState.Spawn);
            initialSpawnWaves=false;
        }
        if (next)
        {
            timer += Time.deltaTime;
            if(timer>duration)
            {
                _boss.changeState(BossState.Jump);
                next = false;
                timer = 0;
            }
        }


    }
}
