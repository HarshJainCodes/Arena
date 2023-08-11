using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BossStateInterface
{
    float timer = 0;
    public override void update(BossMain boss)
    {
        timer += Time.deltaTime;
        if(Vector3.Magnitude(boss.Target.position-boss.transform.position)<15f)
        {
            //code for damaging the player
            //also change state after.
        }
        if(timer>boss.AttackTime)
        {
            boss._animationController.SetTrigger("GlobalInterrupt");
            timer = 0;
        }
        
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("Attack");
    }

    public override void exit(BossMain bossAgent)
    {
        timer = 0;
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
