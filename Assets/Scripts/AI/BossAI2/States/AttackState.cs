using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BossStateInterface
{
    public override void update(BossMain boss)
    {
        if(Vector3.Magnitude(boss.Target.position-boss.transform.position)<15f)
        {
            //code for damaging the player
            //also change state after.
        }
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("Attack");
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
