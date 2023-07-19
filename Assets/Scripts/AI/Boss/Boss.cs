using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LookAtPlayer()
    {
	    Vector3 lookPos = Player.position - transform.position;
	    lookPos.y = 0;
	    Quaternion rotation = Quaternion.LookRotation(lookPos);
	    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);
	}
}
