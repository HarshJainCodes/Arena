using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class EnemyUnit : MonoBehaviour
{
    public AiAgent Agent;
	public Transform refer;

    private void Awake(){
		Agent = GetComponent<AiAgent>();
	}

    public void MoveTo(Vector3 position)
    {
	    float endDistance = Agent.GetComponent<AIPath>().endReachedDistance;
		refer.position = position;

		if (Agent.InRange || Agent.sensor.IsInSight(Agent.PlayerTransform.gameObject))
	    { 
			Agent.GetComponent<AIPath>().endReachedDistance = 0f;
			Agent.GetComponent<AIDestinationSetter>().target = refer;
	    }
		else
		{
			Agent.GetComponent<AIPath>().endReachedDistance = endDistance;
			Agent.GetComponent<AIDestinationSetter>().target = Agent.PlayerTransform;
			Agent.StateMachine.ChangeState(AiStateType.Chase);
		}
	}
}
