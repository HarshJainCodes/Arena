using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossStateInterface
{
    // Start is called before the first frame update

    public override void update(BossMain boss)
    {
        
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("Idle");
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
