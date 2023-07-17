using Pathfinding;

namespace AI.Lev_1_AI.AI_State_Machine.States
{
	public class AiPatrolState : AiState
	{
		public AiAgent Agent;
		public float range = 5f;

		public AiStateType GetStateType()
		{
			return AiStateType.Patrol;
		}

		public void Enter(AiAgent agent)
		{
			agent.GetComponent<AIDestinationSetter>().enabled = true;
			agent.GetComponent<AIPath>().enabled = true;
			agent.GetComponent<Seeker>().enabled = true;
		}

		public void Update(AiAgent agent)
		{
			agent.GetComponent<AIDestinationSetter>().enabled = true;
			agent.GetComponent<AIPath>().enabled = true;
			agent.GetComponent<Seeker>().enabled = true;
			

		}

		public void Exit(AiAgent agent)
		{
			agent.GetComponent<AIDestinationSetter>().enabled = false;
			agent.GetComponent<AIPath>().enabled = false;
			agent.GetComponent<Seeker>().enabled = false;
		}
	}
}