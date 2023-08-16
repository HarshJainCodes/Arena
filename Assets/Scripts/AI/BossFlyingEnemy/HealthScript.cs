using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float health = 40;
    [SerializeField] DestroyFlyingEnemy _dest;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void takeDamage(float dmg)
    {
        health = health - dmg;
        if(health <= 0)
        {
            _dest.destroySelf();
        }
    }
}
