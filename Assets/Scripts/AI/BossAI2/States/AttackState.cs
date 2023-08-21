using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BossStateInterface
{
    float timer = 0;
    bool triggeronce = false;
    public override void update(BossMain boss)
    {
        
        timer += Time.deltaTime;
        if(Vector3.Magnitude(boss.Target.position-boss.transform.position)<15f)
        {
            if (!triggeronce)
            {
                triggeronce = true;
                boss.Target.gameObject.GetComponent<PlayerHealth>().DamagePlayer(20);
            }
            //code for damaging the player
            //also change state after.
        }
        if(timer>boss.AttackTime)
        {
            //boss.changeState(BossState.Attack);
            //boss._animationController.SetTrigger("GlobalInterrupt");
            //boss._stateMachine.GlobalInterrupt = true;
            boss._stateMachine.ChangeState(BossState.Chase);
            timer = 0;
        }
        
    }

    public override void enter(BossMain bossAgent)
    {
        triggeronce = false;
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("Attack");
    }

    public override void exit(BossMain bossAgent)
    {
        timer = 0;
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
