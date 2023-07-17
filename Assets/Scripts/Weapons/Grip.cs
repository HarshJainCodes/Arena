using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;

    public  Sprite GetSprite() =>sprite;
}
