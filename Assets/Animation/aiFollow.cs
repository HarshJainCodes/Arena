using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Ionic.Zlib;
using UnityEngine;


public class aiFollow : MonoBehaviour
{
	// public Transform playerTransform;
	private AIPath aiPath;
	private Animator animator;
	public float a;
	public float SpeedMax=10;

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
		aiPath = GetComponentInChildren<AIPath>();
		aiPath.maxSpeed = SpeedMax;
    }

    // Update is called once per frame
    void Update()
    {
	    a = aiPath.velocity.magnitude;
		animator.SetFloat("Speed", aiPath.velocity.magnitude);
    }
}
