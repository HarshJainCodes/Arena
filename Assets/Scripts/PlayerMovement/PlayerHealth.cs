using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float Health;
    private float MaxHealth=100;

    public event EventHandler<float> OnHealthBarChanged;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        AddHealthToPlayer(0);
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
            AudioManager.instance?.musicEventInstance.setParameterByName("WaveMusic", 5f);
            Debug.Log("player died");
        }
    }

    private void Update()
    {
        if(Health<=0)
        {
            SceneManager.LoadScene("DeathScreen");
        }
    }
}
