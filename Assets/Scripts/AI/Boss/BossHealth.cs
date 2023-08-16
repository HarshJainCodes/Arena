using Pathfinding;
using Pathfinding.RVO.Sampled;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BossHealth : Enemy
{
	[SerializeField] private float timeToDie = 3;
	AIPath aiPath;
	public BossMain BossAgent;
	private bool _IsDead = false;
	public bool IsInvulnerable = false;
	public bool runonce = true;

	// private EnemyDrop _EnemyDropScript;

	private void Awake()
	{
		// Get references to components on the boss enemy
		//BossAgent = GetComponent<AiBossAgent>(); // This is the main script that is attached on the boss game object
		aiPath = GetComponent<AIPath>();
		// _EnemyDropScript = GetComponent<EnemyDrop>();
	}

	// Function to handle boss enemy damage
	public override void Damage(float damage)
	{
		if (_IsDead) return;
		if (IsInvulnerable) return;

		// Call the base class Damage function to apply the damage
		base.Damage(damage);
		// Additional logic for damage handling can be added here
	}

	// Function to handle boss enemy death
	public override void Die()
	{
		if (runonce)
		{
            runonce = false;
            Debug.LogError("death");
			if (_IsDead) return;
			_IsDead = true;

			// Disable the Box collider to prevent further interactions
			//GetComponent<BoxCollider>().enabled = false;

			// Disable any health or shield sliders (if they exist)
			if (shieldSlider != null) shieldSlider.gameObject.SetActive(false);
			if (healthSlider != null) healthSlider.gameObject.SetActive(false);

			BossAgent.changeState(BossState.Death);
			// Invoke the onDie function after the specified time to trigger the death effect
			Invoke("onDie", timeToDie); // This is to allow the death animation to play

			// Disable AI-related components to prevent any further actions
			GetComponent<AIDestinationSetter>().enabled = false;
			GetComponent<AIPath>().enabled = false;
			GetComponent<Seeker>().enabled = false;
			
		}
		// Change the boss state to the dead state
		//BossAgent.StateMachine.ChangeState(AiBossStateType.Dead);
	}

	// Function called after the specified time to destroy the boss game object and perform any other cleanup
	private void onDie()
	{
        Debug.LogError("deathDestroy");
        Destroy(BossAgent.gameObject);

        //StartCoroutine(deathTimer());

        // Destroy the boss game object along with its parent
        //Destroy(gameObject.transform.parent.gameObject);

        // Optionally, drop items or perform other actions when the boss dies
        // _EnemyDropScript.Drop((EnemyDrop.EnemyDrops)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyDrop.EnemyDrops)).Length));
    }
}
