using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DashState : BossStateInterface
{
    float timer = 0;
    bool triggeronce = true;
    float speed = 150f;
    public override void update(BossMain boss)
    {
        timer += Time.deltaTime;
        if (timer > boss.DashTime)
        {
            //Debug.LogError("Here");
            boss._animationController.SetBool("Dashed", true);
            timer = 0;
            boss._stateMachine.GlobalInterrupt = true;
            boss.changeState(BossState.Attack);
        }
        RaycastHit hit;
        if (triggeronce)
        { 
            if (Physics.Raycast(boss.transform.position, (boss.Target.position - boss.transform.position), out hit, 100f))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    triggeronce = false;
                    Debug.Log("Boss can Dash");
                    boss.Aipath.enabled = false;
                    boss._animationController.SetTrigger("Dash");
                    boss._animationController.SetBool("Dashed", false);
                }
            }
        }
        else if(Vector3.Magnitude(boss.Target.position - boss.transform.position)>10f)
        {
            boss.CharController.Move((boss.Target.position - boss.transform.position) / Vector3.Magnitude(boss.Target.position - boss.transform.position) * speed * Time.deltaTime);
        }
    }

    public override void enter(BossMain bossAgent)
    {
        
    }

    public override void exit(BossMain bossAgent)
    {
        timer = 0;
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
        //bossAgent._animationController.SetBool("Dashed", false);
    }


}
