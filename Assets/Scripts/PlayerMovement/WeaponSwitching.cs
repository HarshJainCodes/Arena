using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private Key[] key;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    private void Start()
    {
        SetWeapon();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }
    private void Update()
    {
        int previousSelectedWeapons = selectedWeapon;
        for(int i=0;i<key.Length;i++)
        {
            if(Keyboard.current[key[i]].wasPressedThisFrame && timeSinceLastSwitch>=switchTime)
            {
                selectedWeapon = i;
            }
        }
        if (previousSelectedWeapons != selectedWeapon) Select(selectedWeapon);
        timeSinceLastSwitch += Time.deltaTime;
    }
    private void SetWeapon()
    {
        weapons = new Transform[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            weapons[i] = transform.GetChild(i);
        }
        if (key == null) key = new Key[weapons.Length];
    }

    private void Select(int weaponIndex)
    {
        for(int i=0;i<weapons.Length;i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);

        }
        timeSinceLastSwitch = 0f;
        OnWeaponSelected();
    }

    private void OnWeaponSelected()
    {
        
    }
}
