using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpOnContact : MonoBehaviour
{
    /// <summary>
    /// This variable will store the players transform  
    /// </summary>
    GameObject _Player;
    /// <summary>
    /// Time in float before the enemy explodes as soon as it gets within a radius
    /// </summary>
    [SerializeField] float _TimeBeforeExploding=1f;
    /// <summary>
    /// Enemy explode radius. The radius within which the enemy will explode next to the player.
    /// </summary>
    [SerializeField] int _DetonationDistance=2;

    /// <summary>
    /// The purpose of this variable is to only trigger the coroutine once.
    /// </summary>
    bool _triggered = false;

    [SerializeField] AIPath _pathfinding;

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if(_checkDistance() && !_triggered)
        {
            _triggered = true;
            StartCoroutine(detonate());
        }
    }

    /// <summary>
    /// This function returns true if the enemy is close enough to the player to detonate.
    /// </summary>
    /// <returns>bool</returns>
    private bool _checkDistance()
    {
        //Debug.LogError(gameObject.transform.position);

        if(Vector3.Magnitude((_Player.transform.position - gameObject.transform.position))<_DetonationDistance)
        {
            return true;
        }
        else
        { 
            return false; 
        }
        
    }

    /// <summary>
    /// This coroutine is triggered to destroy the enemy itself.
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator detonate()
    {
        
        _pathfinding.enabled = false;
        gameObject.GetComponentInParent<Animator>().SetBool("Explode", true);
        yield return null;
        //gameObject.transform.GetComponent<Animator>().SetBool("Jump",true);
        //yield return new WaitForSeconds(_TimeBeforeExploding);
        //Destroy(GetComponentInParent<Transform>().gameObject);
    }
}
