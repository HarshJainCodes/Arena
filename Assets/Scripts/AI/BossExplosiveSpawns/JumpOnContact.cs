using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpOnContact : MonoBehaviour
{
    /// <summary>
    /// This variable will store the players transform  
    /// </summary>
    Transform _Player;
    /// <summary>
    /// Time in float before the enemy explodes as soon as it gets within a radius
    /// </summary>
    [SerializeField] float _TimeBeforeExploding=1f;
    /// <summary>
    /// Enemy explode radius. The radius within which the enemy will explode next to the player.
    /// </summary>
    [SerializeField] int _DetonationDistance=2;

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(_checkDistance())
        {
            StartCoroutine(detonate());
        }
    }

    /// <summary>
    /// This function returns true if the enemy is close enough to the player to detonate.
    /// </summary>
    /// <returns>bool</returns>
    private bool _checkDistance()
    {
        if(Vector3.Magnitude((_Player.position - gameObject.transform.position))<_DetonationDistance)
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
        gameObject.transform.GetComponent<Animator>().SetBool("Jump",true);
        yield return new WaitForSeconds(_TimeBeforeExploding);
        Destroy(gameObject);
    }
}
