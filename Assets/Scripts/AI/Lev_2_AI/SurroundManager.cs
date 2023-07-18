using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class SurroundManager : MonoBehaviour
{
    private static SurroundManager _instance;

    public static SurroundManager Instance
    {
	    get
	    {
		    return _instance;
	    }
	    private set
	    {
			_instance = value;
	    }
    }

	public Transform Target;
	public float Radius = 3f;
	public List<EnemyUnit> Units = new List<EnemyUnit>();

	private void Awake() 
	{
		if (Instance == null)
		{
			Instance = this;
			return;
		}
		Destroy(gameObject);
	}

	private void Update()
	{
		// for (int i = 0; i < Units.Count; i++)
		// {
		// 	if (Units[i].Agent.stateMachine.currentStateType != AiStateType.AttackSurround)
		// 	{
		// 		Units.RemoveAt(i);
		// 		i--;
		// 	}
		// }
		MakeAgentsCircleTarget();
	}

	private void MakeAgentsCircleTarget()
	{
		for (int i = 0; i < Units.Count; i++)
		{
			var position = Target.position;
			Units[i].MoveTo(new Vector3(
						position.x + Radius * Mathf.Cos(2 * Mathf.PI * i / Units.Count),
						position.y,
						position.z + Radius * Mathf.Sin(2 * Mathf.PI * i / Units.Count)
					));
		}
	}
}
