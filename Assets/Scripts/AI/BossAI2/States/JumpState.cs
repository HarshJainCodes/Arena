using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BossStateInterface
{
    float timer = 0;
    bool runOnce = true;
    bool moveToPoint=false;
    Vector3 destination;
    Vector3 start;
    float reachTime = 2f;
    float horizontalSpeed = 0f;
    float verticalSpeed = 0f;
    public override void update(BossMain boss)
    {
        timer += Time.deltaTime;
        //code for jump state
        if(timer>boss.JumpTime)
        {
            timer = 0;
            boss._animationController.SetBool("Jumped", true);
            boss.changeState(BossState.Chase);
        }
        if(runOnce)
        {
            runOnce = false;
            float radius =20f;
            start = boss.transform.position;
            Vector3 location = boss.Target.position;
            location.x = Random.Range(location.x - radius, location.x + radius);
            location.z = Random.Range(location.z - radius, location.z + radius);
            location.y = location.y + 50;
            RaycastHit hit;
            if (Physics.Raycast(location, Vector3.down, out hit))
            {
                destination = hit.point;
                runOnce = false;
                moveToPoint = true;
                Vector3 d = new Vector3(destination.x, 0f, destination.z);
                Vector3 s = new Vector3(start.x, 0f, start.z);
                horizontalSpeed = Vector3.Magnitude(d - s)/reachTime;
                float vertical = (destination.z - start.z);
                verticalSpeed = ((vertical)-(9.8f / 2f * (reachTime) * (reachTime)))*3.0f;
                Debug.Log(destination);
                Debug.Log(start);
                Debug.Log(verticalSpeed);
                Debug.Log(horizontalSpeed);
            }
        }
        if(moveToPoint)
        {
            Vector3 direction = (destination - start) / Vector3.Magnitude(destination - start);
            boss.CharController.Move(new Vector3(direction.x * horizontalSpeed, verticalSpeed, direction.z * horizontalSpeed)*Time.deltaTime); 
            verticalSpeed = verticalSpeed - 8f;
        }

    }

    public override void enter(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = false;
        //destroy boss agent itself after some time.
        bossAgent._animationController.SetTrigger("Jump");
        bossAgent._animationController.SetBool("Jumped", false);
    }

    public override void exit(BossMain bossAgent)
    {
        bossAgent.Aipath.enabled = true;
        timer = 0;
        bossAgent._stateMachine.GlobalInterrupt = true;
        bossAgent._animationController.SetTrigger("GlobalInterrupt");
        //bossAgent.changeState(BossState.Chase);
    }
}
