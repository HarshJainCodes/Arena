using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health;
    private int MaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        Health = 60;
        MaxHealth = 100;
    }

    public void AddHealthToPlayer(int amount)
    {
        Health = Mathf.Min(MaxHealth, Health + amount);
    }

    public void DamagePlayer(int amount)
    {
        Health = Mathf.Max(0, Health -  amount);

        if (Health <= 0)
        {
            Debug.Log("player died");
        }
    }
}
