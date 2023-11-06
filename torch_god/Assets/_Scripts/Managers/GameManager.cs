using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

//This manager singleton does not destroy through scene loads and keeps track of which levels the player is on and will switch scenes accordingly
public class GameManager : PersistentSingleton<GameManager>
{
    public static event Action<int> onBeforeSceneChanged;
    public static event Action<int> onAfterSceneChanged;

    public int currentScene { get; private set; }
    public int startscreen;
    public int level1Scene;

    public Character currentCharacter;
    [SerializeField] private ScriptableSaveData saveData;
    public bool encryptPlayerData;

    [SerializeField] private bool encryptGameData;
    private long loadTime;

    private long saveTime;

    public int level;
    private IDataService dataService = new JsonDataService();
    protected override void Awake()
    {
        base.Awake();
        
       
    }
    private void Start()
    {
        if (!File.Exists(Application.persistentDataPath+"/player-data.json"))
        {
            SerializeJson("/player-data.json", saveData.playerData, encryptPlayerData);
        }
        DeserializePlayerData();
        currentCharacter = saveData.playerData.character;
        level = saveData.playerData.currentGameLevel;
        if (level == 0)
            level = 1;
        
    }

    public void DeserializePlayerData()
    {
        long startTime = DateTime.Now.Ticks;
        try
        {
            var data = dataService.LoadData<SaveData>("/player-data.json", encryptPlayerData);
            saveData.playerData = data;
            loadTime = DateTime.Now.Ticks - startTime;
            //Debug.Log($"Load Time: {(loadTime / TimeSpan.TicksPerMillisecond):N4}ms");

        }
        catch (Exception e)
        {
            Debug.LogError($"Could not read file!");
            throw e;
        }

    }


    public void SerializeJson<T>(string jsonFileName, T data, bool encrypt)
    {
        long startTime = DateTime.Now.Ticks;
        if (dataService.SaveData(jsonFileName, data, encrypt))
        {
            saveTime = DateTime.Now.Ticks - startTime;
            //Debug.Log($"Save Time: {(saveTime / TimeSpan.TicksPerMillisecond):N4}ms");
        }
        else
        {
            Debug.LogError("Could not save file!");
        }
    }

    public void IncrementLevel()
    {
        level++;
        saveData.playerData.currentGameLevel = level;       
    }

    public void NewScene()
    {
        UpdateScene(level);
    }

    public void UpdateScene(int newScene)
    {
        onBeforeSceneChanged?.Invoke(newScene);
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
                Level2();
                break;
            case 3:
                Level3();
                break;
            default:
                break;
        }

        onAfterSceneChanged?.Invoke(newScene);
    }  

    public void MainMenu()
    {
        SceneManager.LoadScene(startscreen);
        AudioManager.Instance.PlayMusic("MenuMusic");
    }

    private void StopMenuMusic()
    {
        AudioManager.Instance.musicSource.Stop();
    }
 
    public void Level1()
    {
        //level 1 consists of 5 floors
        SceneManager.LoadScene(level1Scene);
       
    }
    public void Level2()
    {
        //TODO
    }
    public void Level3()
    {
        //TODO
    }
    public void Level4()
    {
        //TODO
    }

    private void OnDestroy()
    {
        SerializeJson("/player-data.json", saveData.playerData, encryptPlayerData);

    }
}
