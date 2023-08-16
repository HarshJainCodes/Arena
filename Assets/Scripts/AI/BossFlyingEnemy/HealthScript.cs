using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    private float health = 40;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health<=0)
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(float dmg)
    {
        health = health - dmg;
    }
}
