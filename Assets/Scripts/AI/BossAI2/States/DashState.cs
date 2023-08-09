using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BossStateInterface
{
    public override void update(BossMain boss)
    {
        RaycastHit hit;
        if (Physics.Raycast(boss.transform.position,(boss.Target.position-boss.transform.position),out hit,100f))
        {
            if(hit.transform.CompareTag("Player"))
            {
                Debug.Log("Boss can Dash");
                //enable animation
                //keep some wait
                //change state
            }
        }
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        bossAgent._animationController.SetTrigger("Dash");
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
    }
}
