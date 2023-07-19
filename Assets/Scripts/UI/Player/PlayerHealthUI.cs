using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField] private Image _HealthBarImage;
    [SerializeField] private PlayerHealth playerHealthScript;

    Material HealthBarMat;

    // Start is called before the first frame update
    void Start()
    {
        _HealthBarImage.color = Color.green;
        playerHealthScript.OnHealthBarChanged += PlayerHealthScript_OnHealthBarChanged;

        HealthBarMat = _HealthBarImage.material;
        HealthBarMat.SetFloat("_PlayerHealth", 0.6f);
    }

    private void PlayerHealthScript_OnHealthBarChanged(object sender, float e)
    {
        HealthBarMat.SetFloat("_PlayerHealth", e/100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
