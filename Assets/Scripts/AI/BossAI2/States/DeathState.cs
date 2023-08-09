using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BossStateInterface
{
    public override void update(BossMain boss)
    {
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("Death");
        //StartCoroutine(deleteBoss(bossAgent));
        //destroy boss agent itself after some time.
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }

    IEnumerator deleteBoss(BossMain boss)
    {
        yield return new WaitForSeconds(1f);
        Destroy(boss.gameObject);
    }
}
