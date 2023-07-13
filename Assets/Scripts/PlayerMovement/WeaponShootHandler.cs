using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class WeaponShootHandler : MonoBehaviour
{
    [SerializeField]
    private Key reloadKey;
    public static Action shootInput;
    public static Action reloadInput;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            shootInput?.Invoke();
        }
        if(Keyboard.current[reloadKey].wasPressedThisFrame)
        {
            reloadInput?.Invoke();
        }
    }
}
