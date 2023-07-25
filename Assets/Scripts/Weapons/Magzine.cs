using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magzine : MonoBehaviour
{

    [Tooltip("Total Ammunition.")]
    [SerializeField]
    private int ammunitionTotal = 10;


    [Tooltip("Interface Sprite.")]
    [SerializeField]
    private Sprite sprite;
    // Start is called before the first frame update
    public  int GetAmmunitionTotal() => ammunitionTotal;
    /// <summary>
    /// Sprite.
    /// </summary>
    public  Sprite GetSprite() => sprite;
}
