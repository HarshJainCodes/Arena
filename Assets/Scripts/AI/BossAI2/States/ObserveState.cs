using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserveState : BossStateInterface
{
    public override void update(BossMain boss)
    {
        Vector3 lookAtObj = new Vector3(boss.Target.position.x,boss.transform.position.y,boss.Target.position.z);
        boss.gameObject.transform.LookAt(lookAtObj);
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
