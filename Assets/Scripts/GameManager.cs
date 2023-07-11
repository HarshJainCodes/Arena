using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // singleton pattern for the game manager
    public static GameManager Instance { get; private set; }

    private enum State
    {
        LevelBuildUp,
        LevelBuildUpFinished,
        PlayerViewingArena,
        PlayerViewingArenaFinished,
        GamePlaying
    }

    private State state;

    private bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        state = State.LevelBuildUp;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.LevelBuildUp:
                break;
            case State.LevelBuildUpFinished:
                break;
            case State.PlayerViewingArena: 
                break;
            case State.PlayerViewingArenaFinished:
                break;
            case State.GamePlaying:
                break;
        }
    }
}
