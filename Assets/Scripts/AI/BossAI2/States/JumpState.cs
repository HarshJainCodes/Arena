using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BossStateInterface
{
    public override void update(BossMain boss)
    {
        //code for jump state
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        //destroy boss agent itself after some time.
        bossAgent._animationController.SetTrigger("Jump");
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
