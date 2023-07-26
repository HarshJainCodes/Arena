using System;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.Lev_1_AI.AI_State_Machine.States
{
	public class AiPatrolState : IAiState
	{
		public AiAgent Agent;
		public AIPath AiPath;
		public AIPath ai => Agent.GetComponent<AIPath>();
		public float range = 10f;
		public float endDistance;
		public float t = 8f;
		private Vector3 _Spawn;

		public AiStateType GetStateType()
		{
			return AiStateType.Patrol;
		}

		public void Enter(AiAgent agent)
		{
			Agent = agent;
			_Spawn = Agent.transform.position;
			endDistance = Agent.GetComponent<AIPath>().endReachedDistance;
			AiPath = Agent.GetComponent<AIPath>();
			Agent.GetComponent<AIDestinationSetter>().enabled = true;
			Agent.GetComponent<AIPath>().enabled = true;
			Agent.GetComponent<Seeker>().enabled = true;
			AiPath.maxSpeed = agent.PatrolSpeed;
			MoveTo(PickRandomPoint());
		}

		public void Update(AiAgent agent)
		{
			Agent.GetComponent<AIDestinationSetter>().enabled = true;
			Agent.GetComponent<AIPath>().enabled = true;
			Agent.GetComponent<Seeker>().enabled = true;

			if (Agent.sensor.IsInSight(Agent.PlayerTransform.gameObject) || Agent.InRange)
			{
				AiPath.maxSpeed = agent.Speed;
				Agent.GetComponent<AIPath>().endReachedDistance = endDistance;
				Agent.GetComponent<AIDestinationSetter>().target = Agent.PlayerTransform;
				Agent.StateMachine.ChangeState(AiStateType.Chase);
			}

			if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
			{
				// t -= Time.deltaTime;
				// if (t <= 0)
				{
					MoveTo(PickRandomPoint());
					// t = 8f;
				}
				ai.SearchPath();
			}
		}

		public void Exit(AiAgent agent)
		{
			AiPath.maxSpeed = 4f;
			Agent.GetComponent<AIPath>().endReachedDistance = endDistance;
			Agent.GetComponent<AIDestinationSetter>().target = Agent.PlayerTransform;
			Agent.GetComponent<AIDestinationSetter>().enabled = false;
			Agent.GetComponent<AIPath>().enabled = false;
			Agent.GetComponent<Seeker>().enabled = false;
		}

		Vector3 PickRandomPoint()
		{
			Vector3 point = Random.insideUnitSphere * range;

			point.y = 0;
			point += Agent.transform.position;
			while(DistanceFromSpawn(point) >= 25f)
			{
				point = Random.insideUnitSphere * range;
				point.y = 0;
				point += Agent.transform.position;
			}
			return point;
		}

		public void MoveTo(Vector3 position)
		{
			Agent.PatrolPoint.position = position;

			// if (ai.velocity.magnitude == 0)
			// {
			// 	return;
			// }

			if (!Agent.sensor.IsInSight(Agent.PlayerTransform.gameObject) && !Agent.InRange)
			{
				Agent.GetComponent<AIPath>().endReachedDistance = 1f;
				Agent.GetComponent<AIDestinationSetter>().target = Agent.PatrolPoint;
			}
			else
			{
				AiPath.maxSpeed = Agent.Speed;
				Agent.GetComponent<AIPath>().endReachedDistance = endDistance;
				Agent.GetComponent<AIDestinationSetter>().target = Agent.PlayerTransform;
				Agent.StateMachine.ChangeState(AiStateType.Chase);
			}
		}

		private float DistanceFromSpawn(Vector3 point)
		{
			return (float)(Math.Sqrt(Math.Pow(point.x - _Spawn.x, 2) + Math.Pow(point.z - _Spawn.z, 2)));
		}
	}
}