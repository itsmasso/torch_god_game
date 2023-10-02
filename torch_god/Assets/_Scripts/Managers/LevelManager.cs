using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

//all the states
public enum GameState
{
    Wave1,
    Wave2,
    Wave3,
    Wave4,
    Wave5,
    LevelUp,
    EndOfFloor,
    PlayerDied
}

//This singleton class will handle all of the game states of a level. For this class in particular, it will handle
//all levels that spawn waves of enemies, level ups, player death, and level finishes. 
public class LevelManager : Singleton<LevelManager>
{
    //bool for if you want to spawn in a test player
    [SerializeField]
    private bool testPlayer;

    private int timePerWave;
    [SerializeField] private int totalTime = 300;

    [SerializeField]
    private TMP_Text currentLevel;
    [SerializeField]
    private TMP_Text currentXP;

    private Vector2 playerStartPosition;

    public float minXBounds, maxXBounds, minYBounds, maxYBounds;

    public static event Action<GameState> onBeforeStateChanged;
    public static event Action<GameState> onAfterStateChanged;

    public event Action onLevelUp;
    [SerializeField] private GameObject levelUpScreen;
    [SerializeField] private GameObject restartScreen;
    [SerializeField] private GameObject pauseScreen;

    private bool wave1Triggered, wave2Triggered, wave3Triggered, wave4Triggered, wave5Triggered = false;

    public event Action onWave1;
    public event Action onWave2;
    public event Action onWave3;
    public event Action onWave4;
    public event Action onWave5;
    public event Action onEndOfFloor;
    public event Action onPlayerDied;

    public GameState state { get; private set; }
    public ScriptablePlayerUnit playerScriptable;
    public ScriptablePlayerData playerData;
    public GameObject player;

    public GameState currentWave { get; private set; }
    public float timer { get; private set; }
    public TMP_Text timeText;
    private bool timeIsRunning;

    public bool isPaused;

    protected override void Awake()
    {
        base.Awake();
        timePerWave = totalTime / 5;
        if (testPlayer)
        {
            playerScriptable = ResourceSystem.Instance.GetPlayerCharacter(Character.TorchCharacter);
        }
        else
        {
            playerScriptable = ResourceSystem.Instance.GetPlayerCharacter(GameManager.Instance.currentCharacter);
        }
        playerStartPosition = new Vector2((maxXBounds-minXBounds)/2+minXBounds, (maxYBounds - minYBounds) / 2 + minYBounds);
        player = Instantiate(playerScriptable.characterPrefab, (Vector3)playerStartPosition, Quaternion.identity);
    }
    private void Start()
    {
       
        timer = totalTime;
        Time.timeScale = 1;
        restartScreen.SetActive(false);
        pauseScreen.SetActive(false);
        timeIsRunning = true;

        isPaused = false;

        playerData = ResourceSystem.Instance.persistentPlayerData;
        ResetPlayerData();
        StartCoroutine(ChangeWaveState());
        UpdateGameState(GameState.Wave1);
    }
    //updates gamestates
    public void UpdateGameState(GameState newState)
    {
        onBeforeStateChanged?.Invoke(newState);
        state = newState;
        
        switch (newState)
        {
            case GameState.Wave1:
                levelUpScreen.SetActive(false);
                HandleWave1();
                
                break;
            case GameState.Wave2:
                levelUpScreen.SetActive(false);
                HandleWave2();
                break;
            case GameState.Wave3:
                levelUpScreen.SetActive(false);
                HandleWave3();
                break;
            case GameState.Wave4:
                levelUpScreen.SetActive(false);
                HandleWave4();
                break;
            case GameState.Wave5:
                levelUpScreen.SetActive(false);
                HandleWave5();
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
        //pause game and open level up screen
        //after choosing new upgrade, resume game and updateGamestate based on current wave
    }

    //functions for handling states
    private void HandleWave1()
    {
        
        if (!wave1Triggered)
        {
            Debug.Log("Wave 1");
            currentWave = GameState.Wave1;
            onWave1?.Invoke();
            wave1Triggered = true;
        }
        
    }

    private void HandleWave2()
    {    
        if (!wave2Triggered)
        {
            Debug.Log("Wave 2");
            currentWave = GameState.Wave2;
            onWave2?.Invoke();
            wave2Triggered = true;
        }
    }

    private void HandleWave3()
    {
        if (!wave3Triggered)
        {
            Debug.Log("Wave 3");
            currentWave = GameState.Wave3;
            onWave3?.Invoke();
            wave3Triggered = true;
        }
    }
    private void HandleWave4()
    {
        if (!wave4Triggered)
        {
            Debug.Log("Wave 4");
            currentWave = GameState.Wave4;
            onWave4?.Invoke();
            wave4Triggered = true;
        }
    }
    private void HandleWave5()
    {
        if (!wave5Triggered)
        {
            Debug.Log("Wave 5");
            currentWave = GameState.Wave5;
            onWave5?.Invoke();
            wave5Triggered = true;
        }
    }

    private void HandleEndOfFloor()
    {
        Debug.Log("End of floor");
        onEndOfFloor?.Invoke();
    }
    private void ResetPlayerData()
    {
        playerData.health = 0;
        playerData.attack = 0;
        playerData.critChance = 0;
        playerData.currentLevel = 1;
        playerData.pickUpRange = 0;
        playerData.movementSpeed = 0;
        playerData.currentExperience = 0;
        playerData.maxExperience = 0;
        playerData.currentUpgrades.Clear();
    }
    private void HandlePlayerDied()
    {     
        restartScreen.SetActive(true);
        Time.timeScale = 0;

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
        currentLevel.text = string.Format("Level: {0}", playerData.currentLevel);
        currentXP.text = string.Format("{0} / {1}", playerData.currentExperience, playerData.maxExperience);
  
    }

    private IEnumerator ChangeWaveState()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(timePerWave);
            if(currentWave != GameState.Wave5)
            {
                UpdateGameState(currentWave + 1);
            }
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
        
    }
}
