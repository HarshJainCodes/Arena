using DG.Tweening;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : BossStateInterface
{
    private int noOfMinionsPerWave=10;
    private float timerBetweenEachSpawn = 1f;
    private float timer = 0f;
    private int currentMinionNumber = 0;
    

    public override void update(BossMain boss)
    {
        timer += Time.deltaTime;
        if(timer>=timerBetweenEachSpawn)
        {
            boss.explosion.SetActive(true);
            GameObject created = Instantiate(boss.currentSpawn, boss.transform.position, Quaternion.identity,boss.minionsParent);
            JumpMath jm=created.AddComponent<JumpMath>();
            if (created.GetComponentInChildren<AIDestinationSetter>() != null)
                created.GetComponentInChildren<AIDestinationSetter>().target = boss.Target;
            jm.Starting = boss.transform.position;
            jm.Destination = new Vector3(boss.Target.position.x, boss.Target.position.y + 10f, boss.Target.position.z);
            jm.Control = new Vector3(boss.Target.position.x , boss.Target.position.y + Random.Range(50f, 200f), boss.Target.position.z);
            jm.Timing =3f;
            jm.Set = true;
            currentMinionNumber++;
            Vector3 m = new Vector3(boss.Target.position.x, boss.transform.position.y, boss.Target.position.z);
            created.transform.LookAt(m);
            timer = 0;
            if (currentMinionNumber == noOfMinionsPerWave)
            {
                boss.waveNo++;
                boss.changeState(BossState.Observe);
            }
        }
        //code for spawning minions
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.bossWeaponTransform.gameObject.SetActive(true);
        if(bossAgent.waveNo==0)
        {
            bossAgent.currentSpawn = bossAgent.enemySpawn;
        }
        if(bossAgent.waveNo==1)
        {
            bossAgent.currentSpawn = bossAgent.flyingSpawn;
        }
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetBool("SpawnEnemies",true);
        //destroy boss agent itself after some time.
    }

    public override void exit(BossMain bossAgent)
    {
        timer = 0;
        currentMinionNumber = 0;
        bossAgent.explosion.SetActive(false);
        bossAgent._animationController.SetBool("SpawnEnemies", false);
        //bossAgent._animationController.SetTrigger("GlobalInterrupt");
        //bossAgent.bossWeaponTransform.gameObject.SetActive(false);

    }
}
