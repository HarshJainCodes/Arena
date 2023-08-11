using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossStateInterface
{
    public override void update(BossMain boss)
    {
        Vector3 lookAtObj = new Vector3(boss.Target.position.x, boss.transform.position.x, boss.Target.position.z);
        boss.gameObject.transform.LookAt(lookAtObj);
        if(!boss.Aipath.enabled)
        {
            boss.Aipath.enabled = true;
        }
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = true;
        bossAgent._animationController.SetTrigger("Chase");
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
