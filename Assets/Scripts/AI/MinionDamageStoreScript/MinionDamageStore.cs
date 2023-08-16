using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDamageStore : MonoBehaviour
{
    public float damage;
    public PlayerHealth ph;
    // Start is called before the first frame update
    void Start()
    {
        damage = 0;
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }
    // Update is called once per frame
    void Update()
    {
        if(damage>0)
        {
            Debug.LogError("Triggered");
            ph.DamagePlayer(damage);
            damage = 0;
        }
    }
}
