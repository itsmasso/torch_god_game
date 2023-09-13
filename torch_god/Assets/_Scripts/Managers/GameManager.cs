using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//all the states
public enum GameState
{
    Start
}

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> onBeforeStateChanged;
    public static event Action<GameState> onAfterStateChanged;
    public GameState state { get; private set; }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    //updates gamestates
    public void UpdateGameState(GameState newState)
    {
        onBeforeStateChanged?.Invoke(newState);
        state = newState;

        switch (newState)
        {
            case GameState.Start:
                HandleStart();
                break;
            default:
                break;
        }

        onAfterStateChanged?.Invoke(newState);
    }

    //functions for states
    public void HandleStart()
    {
        //TODO
    }
}
