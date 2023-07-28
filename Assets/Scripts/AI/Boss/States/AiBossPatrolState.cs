using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiBossPatrolState : IAiBossState
{
    public AiBossAgent BossAgent;
    public AIPath AiPath;
    public AIPath ai => BossAgent.GetComponent<AIPath>();
    public float range = 120f;
    public float endDistance;
    public float t = 8f;
    private Vector3 _Spawn;

	public AiBossStateType GetStateType()
    {
	    return AiBossStateType.Patrol;
    }

    public void Enter(AiBossAgent bossAgent)
    {
	    BossAgent = bossAgent;
        _Spawn = BossAgent.transform.position;
        endDistance = BossAgent.GetComponent<AIPath>().endReachedDistance;
        AiPath = BossAgent.GetComponent<AIPath>();
        BossAgent.GetComponent<AIDestinationSetter>().enabled = true;
        BossAgent.GetComponent<AIPath>().enabled = true;
        BossAgent.GetComponent<Seeker>().enabled = true;
        AiPath.maxSpeed = bossAgent.BossPatrolSpeed;
        MoveTo(PickRandomPoint());
    }

    public void Update(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;
		if (BossAgent.Sensor.IsInSight(BossAgent.PlayerTransform.gameObject) || BossAgent.InRange)
		{
	        AiPath.maxSpeed = BossAgent.BossSpeed;
			BossAgent.GetComponent<AIPath>().endReachedDistance = endDistance;
			BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PlayerTransform;
			BossAgent.StateMachine.ChangeState(AiBossStateType.Chase);
		}

		if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
		{
            MoveTo(PickRandomPoint());
	        ai.SearchPath();
		}
    }

    public void Exit(AiBossAgent bossAgent)
    {
	    AiPath.maxSpeed = bossAgent.BossSpeed;
        BossAgent.GetComponent<AIPath>().endReachedDistance = endDistance;
        BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PlayerTransform;
        BossAgent.GetComponent<AIDestinationSetter>().enabled = false;
        BossAgent.GetComponent<AIPath>().enabled = false;
	    BossAgent.GetComponent<Seeker>().enabled = false;
    }

    Vector3 PickRandomPoint()
    {
	    Vector3 point = Random.insideUnitSphere * range;

	    point.y = 0;
	    point += BossAgent.transform.position;
	    while (DistanceFromSpawn(point) >= 800f)
	    {
		    point = Random.insideUnitSphere * range;
		    point.y = 0;
		    point += BossAgent.transform.position;
	    }
	    return point;
    }

    private float DistanceFromSpawn(Vector3 point)
    {
	    return (float)(Math.Sqrt(Math.Pow(point.x - _Spawn.x, 2) + Math.Pow(point.z - _Spawn.z, 2)));
    }

    public void MoveTo(Vector3 position)
    {
	    BossAgent.PatrolPoint.position = position;

	    BossAgent.GetComponent<AIPath>().endReachedDistance = 1f;
	    BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PatrolPoint;

		if (!BossAgent.Sensor.IsInSight(BossAgent.PlayerTransform.gameObject) && !BossAgent.InRange)
	    {
		    BossAgent.GetComponent<AIPath>().endReachedDistance = 1f;
		    BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PatrolPoint;
	    }
	    else
	    {
		    AiPath.maxSpeed = BossAgent.BossSpeed;
		    BossAgent.GetComponent<AIPath>().endReachedDistance = endDistance;
		    BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PlayerTransform;
		    BossAgent.StateMachine.ChangeState(AiBossStateType.Chase);
	    }
	}
}
