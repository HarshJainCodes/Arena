using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference Walk { get; private set; }
    [field: SerializeField] public EventReference Run { get; private set; }
    [field: SerializeField] public EventReference Crouch { get; private set; }
    [field: SerializeField] public EventReference Jump { get; private set; }
    [field: SerializeField] public EventReference Land { get; private set; }

    [field: SerializeField] public EventReference WallRun { get; private set; }

    [field: SerializeField] public EventReference firing_AR { get; private set; }



    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}