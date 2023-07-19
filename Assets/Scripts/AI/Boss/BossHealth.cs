using Pathfinding;
using Pathfinding.RVO.Sampled;
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BossHealth : Enemy
{
	[SerializeField] private float timeToDie = 3;
	AIPath aiPath;
	public AiBossAgent BossAgent;
	private bool _IsDead = false;
	public bool IsInvulnerable = false;

	private EnemyDrop _EnemyDropScript;

	private void Awake()
	{
		BossAgent = GetComponent<AiBossAgent>();
		aiPath = GetComponent<AIPath>();
		_EnemyDropScript = GetComponent<EnemyDrop>();
	}
	public override void Damage(float damage)
	{
		if (_IsDead) return;
		if(IsInvulnerable) return;
		if (BossAgent.StateMachine.CurrentBossStateType is AiBossStateType.Idle or AiBossStateType.Patrol) BossAgent.StateMachine.ChangeState(AiBossStateType.Chase);
		base.Damage(damage);
	}

	public override void Die()
	{
		if (_IsDead) return;
		_IsDead = true;
		
		// GetComponentInParent<Animator>().Play("DieAnim");

		GetComponent<CapsuleCollider>().enabled = false;

		if (shieldSlider != null) shieldSlider.gameObject.SetActive(false);
		if (healthSlider != null) healthSlider.gameObject.SetActive(false);
		Invoke("onDie", timeToDie);
		GetComponent<AIDestinationSetter>().enabled = false;
		GetComponent<AIPath>().enabled = false;
		GetComponent<Seeker>().enabled = false;
		BossAgent.StateMachine.ChangeState(AiBossStateType.Dead);
	}

	private void onDie()
	{
		Destroy(gameObject.transform.parent.gameObject);
		_EnemyDropScript.Drop((EnemyDrop.EnemyDrops)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyDrop.EnemyDrops)).Length));
	}
}
