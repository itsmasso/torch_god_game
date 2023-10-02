using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This manager singleton does not destroy through scene loads and keeps track of which levels the player is on and will switch scenes accordingly
public class GameManager : PersistentSingleton<GameManager>
{
    public static event Action<int> onBeforeSceneChanged;
    public static event Action<int> onAfterSceneChanged;
    public int currentScene { get; private set; }
    public int startscreen = 0;
    public int level1Scene = 1;

    public Character currentCharacter;

    private void Start()
    {
        currentCharacter = Character.TorchCharacter;
        UpdateScene(startscreen);
    }

    public void UpdateScene(int newScene)
    {
        onBeforeSceneChanged?.Invoke(newScene);
        currentScene = newScene;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        switch (newScene)
        {
            case 0:
                MainMenu();
                break;
            case 1:
                Level1();
                break;
            case 2:
                Level1();
                break;
            default:
                break;
        }

        onAfterSceneChanged?.Invoke(newScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(startscreen);
    }
 
    public void Level1()
    {
        SceneManager.LoadScene(level1Scene);
        
    }

    public void Level2()
    {
        //TODO
    }
    private void Update()
    {
        //Debug.Log(state);
    }
}
