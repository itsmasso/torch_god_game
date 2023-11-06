using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.Universal;

//all the states
public enum GameState
{
    SpawningWaves,
    LevelUp,
    EndOfFloor,
    PlayerDied
}

//This singleton class will handle all of the game states of a level. For this class in particular, it will handle
//all levels that spawn waves of enemies, level ups, player death, and level finishes. 
public class LevelManager : Singleton<LevelManager>
{  
    [Header("Testing")]
    [SerializeField] private bool testPlayer;

    [Header("Player Properties")]
    public ScriptablePlayerUnit playerScriptable;
    public ScriptableSaveData saveData;
    public GameObject player;
    private Vector2 playerStartPosition;

    [Header("Time Properties")]
    [SerializeField] private int totalTime = 300;
    public float timer { get; private set; }
    private bool timeIsRunning;
    private int timePerWave;

    [Header("Map Properties")]
    public Light2D lights;
    public float minXBounds, maxXBounds, minYBounds, maxYBounds;
    [SerializeField] private GameObject ladder;
    [SerializeField] private Transform mapTransform;

    //events
    public event Action onNewWave;
    public event Action onEndOfFloor;
    public event Action onPlayerDied;
    public event Action onLevelUp;
    public static event Action<GameState> onBeforeStateChanged;
    public static event Action<GameState> onAfterStateChanged;
    

    [Header("UI Elements")]
    [SerializeField] private GameObject levelUpScreen;
    [SerializeField] private GameObject restartScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text currentXPText;
    [SerializeField] private TMP_Text currentFloorText;
    public TMP_Text timeText;

    [Header("Level Properties")]
    public bool isPaused;
    public int currentWave = 0;
    public GameState state { get; private set; }
    private bool spawningWaves = false;
    [SerializeField] private int amountOfWaves = 5;
    public int currentFloor;


    protected override void Awake()
    {
        base.Awake();
        timePerWave = totalTime / amountOfWaves;
        if (testPlayer)
            playerScriptable = ResourceSystem.Instance.GetPlayerCharacter(Character.TorchCharacter);
        else
            playerScriptable = ResourceSystem.Instance.GetPlayerCharacter(GameManager.Instance.currentCharacter);
        //init variables
        playerStartPosition = new Vector2((maxXBounds-minXBounds)/2+minXBounds, (maxYBounds - minYBounds) / 2 + minYBounds);
        player = Instantiate(playerScriptable.characterPrefab, (Vector3)playerStartPosition, Quaternion.identity);
        saveData = ResourceSystem.Instance.saveData;
    }
    private void Start()
    {

        //set default timer properties
        isPaused = false;
        timer = totalTime;
        timeIsRunning = true;
        Time.timeScale = 1;

        //set default ui elements
        restartScreen.SetActive(false);
        pauseScreen.SetActive(false);

        //setting level properties
        currentWave++;    
        currentFloor = saveData.playerData.currentFloor;

        if (currentFloor == 0)
            currentFloor = 1;

        StartCoroutine(LevelCycle());
        UpdateGameState(GameState.SpawningWaves);

        //set player properties
        foreach (int id in saveData.playerData.currentUpgradeIDs)
        {
            ReattatchUpgradesToPlayer(id);
        }
    }
    private void ReattatchUpgradesToPlayer(int id)
    {
        GameObject parent = player;
        ScriptableUpgrade upgrade = ResourceSystem.Instance.GetUpgradeByID(id);
        if (upgrade.needAim)
        {
            Weapon weaponReference = parent.GetComponentInChildren<Weapon>();
            GameObject upgradeObj = Instantiate(upgrade.upgradePrefab, weaponReference.transform.position, weaponReference.firePoint.rotation);
            upgradeObj.transform.parent = weaponReference.firePoint.transform;

        }
        else
        {
            GameObject upgradeObj = Instantiate(upgrade.upgradePrefab, parent.transform.position, Quaternion.identity);
            upgradeObj.transform.parent = parent.transform;
        }
    }

    //updates gamestates
    public void UpdateGameState(GameState newState)
    {
        onBeforeStateChanged?.Invoke(newState);
        state = newState;
        
        switch (newState)
        {
            case GameState.SpawningWaves:
                levelUpScreen.SetActive(false);
                HandleSpawningWaves();
                break;
            case GameState.EndOfFloor:
                levelUpScreen.SetActive(false);
                HandleEndOfFloor();
                break;
            case GameState.LevelUp:
                HandleLevelUp();
                break;
            case GameState.PlayerDied:
                HandlePlayerDied();
                break;
            default:
                break;
        }

        onAfterStateChanged?.Invoke(newState);
    }
    private void HandleLevelUp()
    {
        Time.timeScale = 0;
        levelUpScreen.SetActive(true);
        onLevelUp?.Invoke();
        
        //Note: Refer to UpgradeManager script for logic below
        //pause game and open level up screen
        //after choosing new upgrade, resume game
    }

    //functions for handling states
    private void HandleSpawningWaves()
    {      
        if (!spawningWaves)
        {
            onNewWave?.Invoke();
            spawningWaves = true;
        }
        
    }
    private void HandleEndOfFloor()
    {
        Debug.Log("End of floor");
        onEndOfFloor?.Invoke();
        Instantiate(ladder, mapTransform.position, Quaternion.identity);
        
    }   
    private void HandlePlayerDied()
    {     
        restartScreen.SetActive(true);
        ResourceSystem.Instance.ResetPlayerData();
        Time.timeScale = 0;

    }

    public void IncrementFloors()
    {
        currentFloor++;
        saveData.playerData.currentFloor = currentFloor;
        if (currentFloor % 5 == 0)
        {      
            GameManager.Instance.IncrementLevel();
            GameManager.Instance.NewScene();
        }
        else
        {
            GameManager.Instance.NewScene();
        }
        
    }

    //temporary method, will use input system to handle ui inputs
    private void PauseGame()
    {
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
                
            }
        }else if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
                isPaused = false;

            }
        }
    }

    private void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    private void DisplayXP()
    {
        currentLevelText.text = string.Format("Level: {0}", saveData.playerData.currentLevel);
        currentXPText.text = string.Format("{0} / {1}", saveData.playerData.currentExperience, saveData.playerData.maxExperience);
  
    }

    private void DisplayFloor()
    {
        currentFloorText.text = string.Format("Floor {0}", currentFloor);
    }

    private IEnumerator LevelCycle()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(timePerWave);
            currentWave++;
            onNewWave?.Invoke();          
        }
        UpdateGameState(GameState.EndOfFloor);
    }
    private void Update()
    {
        PauseGame();
        if (timeIsRunning)
        {
            timer -= Time.deltaTime;
        }
        if(timer > 0)
        {
            DisplayTime();
        }
        DisplayXP();
        DisplayFloor();

    }


}
