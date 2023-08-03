using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBossSpawnMobsState : IAiBossState
{
    [SerializeField] private GameObject _enemySpawn;
    private float _timer = 4f;
    private Transform _playerTransform;
    private int _force=500;
    private int _noOfEnemies=5;
    private int enemyCounter = 0;
    private float timerTime = 0f;
    public AiBossStateType GetStateType()
    {
        return AiBossStateType.SpawnMobs; // Return the state type as SpawnMobs
    }

    // Function called when entering the Chase state
    public void Enter(AiBossAgent bossAgent)
    {
        _playerTransform = bossAgent.PlayerTransform;
        bossAgent.gameObject.GetComponentInParent<Animator>().SetBool("StartFiring",true);
        _enemySpawn = bossAgent.SpawnedEnemy;
    }

    // Function called during the Chase state's update
    public void Update(AiBossAgent bossAgent)
    {
        timerTime += Time.deltaTime;
        if(timerTime>_timer && enemyCounter<_noOfEnemies)
        {
            /*timerTime = 0;
            enemyCounter++;
            //spawnMinion(bossAgent);*/
            
        }
        if(bossAgent.gameObject.GetComponentInParent<Animator>().GetBool("StartFiring"))
        {
            bossAgent.StateMachine.ChangeState(AiBossStateType.ObservePlayer);
        }
    }

    // Function called when exiting the Chase state
    public void Exit(AiBossAgent bossAgent)
    {
        bossAgent.gameObject.GetComponentInParent<Animator>().SetBool("StartFiring", false);
        bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
        bossAgent.GetComponent<AIPath>().enabled = true;
        bossAgent.GetComponent<Seeker>().enabled = true;
        bossAgent.GetComponentInParent<aiFollow>().enabled = true;
    }

    public void spawnMinion(AiBossAgent bossAgent)
    {
        /*GameObject created =*/ bossAgent.createInstance(_enemySpawn);
        //Rigidbody rb=created.transform.GetComponentInChildren<Rigidbody>();
        //Vector3 direction = (_playerTransform.position - rb.position)/(Vector3.Magnitude(_playerTransform.position - rb.position));
       // rb.AddForce((direction)*_force,ForceMode.Impulse);
    }
}
