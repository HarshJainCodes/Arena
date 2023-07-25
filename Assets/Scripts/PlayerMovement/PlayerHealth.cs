using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health;
    private float MaxHealth;

    public event EventHandler<float> OnHealthBarChanged;

    // Start is called before the first frame update
    void Start()
    {
        Health = 60;
        MaxHealth = 100;
    }

    public void AddHealthToPlayer(float amount)
    {
        Health = Mathf.Min(MaxHealth, Health + amount);
        OnHealthBarChanged?.Invoke(this, Health);
    }

    public void DamagePlayer(float amount)
    {
        Health = Mathf.Max(0, Health - amount);
        OnHealthBarChanged?.Invoke(this, Health);

        if (Health <= 0)
        {
            Debug.Log("player died");
        }
    }
}
