using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinions : MonoBehaviour
{
    [SerializeField] private GameObject _minion;
    [SerializeField] private int _maxEnemies = 5;
    [SerializeField] private float timerLimit=2f;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _player;
    [SerializeField] private int _force;
    [SerializeField] private Transform _spawns;
    private int currnetNo = 0;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        bool startSpawn = bossAnimator.GetBool("StartFiring");
        if(startSpawn && timer>timerLimit)
        {
            currnetNo++;
            timer = 0;
            StartCoroutine(spawn());
            if (currnetNo == _maxEnemies)
            {
                bossAnimator.SetBool("StartFiring", false);
            }
            
            
        }
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(1);
        GameObject created = Instantiate(_minion, _spawnPoint.position, Quaternion.identity, _spawns);
        //created.transform.LookAt(_player);
        created.GetComponentInChildren<AIDestinationSetter>().target = _player;
        Rigidbody rb = created.GetComponentInChildren<Rigidbody>();
        rb.AddForce(created.transform.forward * _force);
        
    }
}
