using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpOnContact : MonoBehaviour
{
    [SerializeField] Transform _Player;
    [SerializeField] float _TimeBeforeExploding=1f;
    [SerializeField] int _DetonationDistance=2;

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
