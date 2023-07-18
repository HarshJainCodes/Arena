using Pathfinding;
using Pathfinding.RVO.Sampled;
using System;
using UnityEngine;

public class TrainingTarget : Enemy
{
    [SerializeField]private float timeToDie = 3;
    AIPath aiPath;
    SpawnManager spawnManager;
    public AiAgent aiAgent;
    private bool isDead = false;
    
    private EnemyDrop _EnemyDropScript;
    
    private void Awake()
    {
        aiAgent = GetComponent<AiAgent>();
        aiPath = GetComponent<AIPath>();
        spawnManager = FindObjectOfType<SpawnManager>();
        _EnemyDropScript = GetComponent<EnemyDrop>();
    }
	public override void Damage(float damage)
    {
        if (isDead) return;
        if(GetComponent<AiAgent>().stateMachine.currentStateType == AiStateType.Idle) GetComponent<AiAgent>().stateMachine.ChangeState(AiStateType.Chase);
        base.Damage(damage);
        aiAgent = GetComponent<AiAgent>();
        // GetComponent<Animator>().Play("Hit_Reaction_2");
    }

    // private void Awake()
    // {
    //     aiAgent = GetComponent<AiAgent>();
    //     aiPath = GetComponent<AIPath>();
    //     _EnemyDropScript = GetComponent<EnemyDrop>();
    // }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;

		events.OnDeath.Invoke();
        GetComponentInParent<Animator>().Play("DieAnim");
        spawnManager.Enemies.Remove(transform.parent.gameObject);

		GetComponent<CapsuleCollider>().enabled = false;
		// aiPath.maxSpeed = 0;

		if (shieldSlider != null)shieldSlider.gameObject.SetActive(false);
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        // if (UI.GetComponent<UIController>().displayEvents)
        // {
        //     UI.GetComponent<UIController>().AddKillfeed(name);
        // }

        // if (transform.parent.GetComponent<CompassElement>() != null) transform.parent.GetComponent<CompassElement>().Remove();
        Invoke("onDie", timeToDie);
        GetComponent<AIDestinationSetter>().enabled = false;
        GetComponent<AIPath>().enabled = false;
        GetComponent<Seeker>().enabled = false;
        aiAgent.stateMachine.ChangeState(AiStateType.Dead); 
    }

	private void onDie()
    {
	    Destroy(gameObject.transform.parent.gameObject);
        _EnemyDropScript.Drop((EnemyDrop.EnemyDrops) UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyDrop.EnemyDrops)).Length));
    }
}
