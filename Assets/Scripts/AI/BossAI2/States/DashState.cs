using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DashState : BossStateInterface
{
    float timer = 0;
    bool triggeronce = true;
    float speed = 150f;
    JumpMath jm;
    float reachTime = 1f;
    public override void update(BossMain boss)
    {
        /*timer += Time.deltaTime;
        if (timer > boss.DashTime)
        {
            //Debug.LogError("Here");
            //boss._animationController.SetBool("Dashed", true);
            //timer = 0;
            *//*
            boss._stateMachine.GlobalInterrupt = true;
            boss.changeState(BossState.Attack);*//*
        }*/
        RaycastHit hit;
        if (triggeronce)
        { 
            if (Physics.Raycast(boss.transform.position, (boss.Target.position - boss.transform.position), out hit, 100f))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Vector3 start = boss.transform.position;
                    Vector3 destination = boss.Target.position;
                    triggeronce = false;
                    Debug.Log("Boss can Dash");
                    boss.Aipath.enabled = false;
                    jm = boss.gameObject.AddComponent<JumpMath>();
                    jm.Destination = destination;
                    jm.Starting = start;
                    jm.Control = new Vector3(destination.x, destination.y+10f, destination.z);
                    jm.Timing = reachTime;
                    jm.Set = true;
                    boss._animationController.SetTrigger("Dash");
                    //boss._animationController.SetBool("Dashed", false);
                }
            }
            else
            {
                boss.changeState(BossState.Chase);
            }
        }
        else if(jm==null)
        {
            boss.changeState(BossState.Attack);
            // boss.CharController.Move((boss.Target.position - boss.transform.position) / Vector3.Magnitude(boss.Target.position - boss.transform.position) * speed * Time.deltaTime);
        }
    }

    public override void enter(BossMain bossAgent)
    {
        triggeronce = true;
    }

    public override void exit(BossMain bossAgent)
    {
        timer = 0;
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
        //bossAgent._animationController.SetBool("Dashed", false);
    }


}
