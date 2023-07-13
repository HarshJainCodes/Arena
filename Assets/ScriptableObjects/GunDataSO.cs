using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Gun",menuName ="Weapon/Gun")]
public class GunDataSO : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    [Tooltip("Reload time in rounds per minute")]
    public float reloadTime;
    [HideInInspector]
    public bool reloading;

}
