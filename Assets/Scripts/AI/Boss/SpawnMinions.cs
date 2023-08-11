using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinions : MonoBehaviour
{
    [SerializeField] private GameObject _minion;
    [SerializeField] private GameObject _flyingMinions;
    private GameObject currentMinion;
    [SerializeField] private int _maxEnemies = 5;
    [SerializeField] private float timerLimit=2f;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private Transform _player;
    [SerializeField] private BossMain _boss;
    [SerializeField] private int _force;
    [SerializeField] private Transform _spawns;
    private int currnetNo = 0;
    private float timer = 0f;
    private bool finalSpawn = false;
    public bool FinalSpawn { get { return finalSpawn; } }
    // Start is called before the first frame update
    void Start()
    {
        currentMinion = _minion;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        bool startSpawn = bossAnimator.GetBool("SpawnEnemies");
        if(startSpawn && timer>timerLimit)
        {
            if (currnetNo != _maxEnemies)
            {
                timer = 0;
                StartCoroutine(spawn(currnetNo));
                currnetNo++;
            }
            if (currnetNo == _maxEnemies && !finalSpawn)
            {
                _boss.changeState(BossState.Observe);
            }
        }
        if(currnetNo==_maxEnemies && currentMinion==_minion)
        {
            finalSpawn = true;
            currentMinion = _flyingMinions;
            currnetNo = 0;
        }
    }

    IEnumerator spawn(int no)
    {
        yield return new WaitForSeconds(1);
        GameObject created = Instantiate(currentMinion, _spawnPoint[no%_spawnPoint.Length].position, Quaternion.identity, _spawns);
        //created.transform.LookAt(_player);
        if(created.GetComponentInChildren<AIDestinationSetter>() != null)
        created.GetComponentInChildren<AIDestinationSetter>().target = _player;
        //Rigidbody rb = created.GetComponentInChildren<Rigidbody>();
        //rb.AddForce(created.transform.forward * _force);
        
    }
}
