using Pathfinding;
using Pathfinding.RVO.Sampled;
using UnityEngine;

public class TrainingTarget : Enemy
{
    [SerializeField]private float timeToDie = 3;
    AIPath aiPath;
    AiAgent aiAgent;
    private bool isDead = false;

    public override void Damage(float damage)
    {
        if (isDead) return;
        if(GetComponent<AiAgent>().stateMachine.currentStateType == AiStateType.Idle) GetComponent<AiAgent>().stateMachine.ChangeState(AiStateType.Chase);
        base.Damage(damage);
        aiAgent = GetComponent<AiAgent>();
        // GetComponent<Animator>().Play("Hit_Reaction_2");
    }
    public override void Die()
    {
        if (isDead) return;
        isDead = true;

		events.OnDeath.Invoke();
        GetComponentInParent<Animator>().Play("DieAnim");
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
	}
}
