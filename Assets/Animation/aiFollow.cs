using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Ionic.Zlib;
using UnityEngine;
using UnityEngine.Serialization;


public class aiFollow : MonoBehaviour
{
	// public Transform playerTransform;
	public AIPath aiPath;
	private Animator animator;
	public float a;
	public AiAgent AiAgent;
	public AiBossAgent BossAgent;

	// Start is called before the first frame update
	void Start()
    {
	    animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
	    //normalizing speed for animation so that if speed is changed, we don't have to change the animation speed
		if (AiAgent != null)
			a = aiPath.velocity.magnitude / AiAgent.Speed;
		else 
		    a = aiPath.velocity.magnitude / BossAgent.BossSpeed;
	    animator.SetFloat("Speed", a);
    }
}
