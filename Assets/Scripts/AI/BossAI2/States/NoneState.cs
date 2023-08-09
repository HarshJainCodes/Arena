using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneState : BossStateInterface
{

    // Update is called once per frame
    public override void update(BossMain boss)
    {
        //update nothing
    }

    public override void enter(BossMain bossAgent)
    {
        //disable various agent features
        bossAgent.Aipath.enabled = false;
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
        //not required for this state
    }
}
