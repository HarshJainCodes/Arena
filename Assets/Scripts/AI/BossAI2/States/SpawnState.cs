using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : BossStateInterface
{
    public override void update(BossMain boss)
    {
        //code for spawning minions
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("Attack");
        //destroy boss agent itself after some time.
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
