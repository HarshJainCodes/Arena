using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BossStateInterface
{
    float timer = 0;
    public override void update(BossMain boss)
    {
        timer += Time.deltaTime;
        if (timer >boss.DashTime)
        {
            //Debug.LogError("Here");
            boss._animationController.SetBool("Dashed", true);
            timer = 0;
            boss._stateMachine.GlobalInterrupt = true;
        }
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
        bossAgent._animationController.SetBool("Dashed", false);
    }

    public override void exit(BossMain bossAgent)
    {
        timer = 0;
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
        //bossAgent._animationController.SetBool("Dashed", false);
    }


}
