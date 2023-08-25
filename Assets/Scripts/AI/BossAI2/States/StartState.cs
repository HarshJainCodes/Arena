using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartState : BossStateInterface
{
    float timer = 0f;
    float maxLimit = 2f;
    bool timerOn = false;
    public override void update(BossMain bossAgent)
    {
        Vector3 loc = bossAgent.Target.position;
        loc.y = bossAgent.transform.position.y;
        bossAgent.transform.LookAt(loc);
        if(bossAgent.CharController.isGrounded)
        {
            timerOn= true;
            bossAgent._animationController.SetTrigger("ImpactLand");
        }
        timer += Time.deltaTime;
        if(timer>maxLimit)
        {
            timer = 0;
            bossAgent.LandingExplosion.SetActive(true);
            bossAgent.particleTrail.SetActive(false);
            bossAgent.changeState(BossState.Spawn);
        }
    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent._animationController.SetTrigger("Flying");
        bossAgent.Aipath.enabled = false;
        JumpMath jm=bossAgent.AddComponent<JumpMath>();
        jm.Starting=bossAgent.transform.position;
        RaycastHit hit;
        Physics.Raycast(bossAgent.Target.position, Vector3.down, out hit, 100f, LayerMask.GetMask("Ground"));
        Vector3 diff = hit.point;
        diff.y += 1;
        jm.Destination = diff;
        jm.Control = diff;
        jm.Timing = 3f;
        jm.Set = true;

    }

    public override void exit(BossMain bossAgent)
    {

    }

}
