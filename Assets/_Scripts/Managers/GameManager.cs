using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using _Scripts.Units.Player;

public class GameManager : StaticInstance<GameManager>
{
//-------------------------------------------------------------------------------------------//
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public int stars = 0;
    public bool isGameOver = false;
    public bool isPaused = false;
    public bool levelComplete = false;
    public bool mustCreateVehicle = false;
    public static bool playerControl = false;

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject createVehiclePanel;
    public GameObject selectGadgetPanel;
    public GameObject levelCompletePanel;
    public GameObject[] Stars = new GameObject[3];
    Player player;
    public GameState State { get; private set; }
//-------------------------------------------------------------------------------------------//
    [Serializable]
    public enum GameState {
        Starting = 0,
        SpawningHeroes = 1,
        SpawningEnemies = 2,
        HeroTurn = 3,
        EnemyTurn = 4,
        Win = 5,
        Lose = 6,
        Menu = 7,
    }
    
//-------------------------------------------------------------------------------------------//
    // Kick the game off with the first state
    void Start() => ChangeState(GameState.Starting);

        // Update is called once per frame
    void Update()
    {
        // if level complete and press space, go to next level
        if (Input.GetKeyDown(KeyCode.Space) && levelComplete)
        {
            LoadNextLevel();
        }
        //if r key is pressed restart scene
        if (Input.GetKeyDown(KeyCode.R) && (isGameOver || isPaused))
        {
            Restart();
            
        }
        // m for menu
        if (Input.GetKeyDown(KeyCode.M) && isGameOver) 
        {
            SceneManager.LoadScene(0); // main menu
        }
        // p for pause
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) 
        {
            Pause();
        }
        if (isPaused && Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
            // SceneManager.LoadScene(0);
        }
        if (createVehiclePanel.activeSelf)
        {
            
        }
    }
//-------------------------------------------------------------------------------------------//
    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawningHeroes:
                HandleSpawningHeroes();
                break;
            case GameState.SpawningEnemies:
                HandleSpawningEnemies();
                break;
            case GameState.HeroTurn:
                HandleHeroTurn();
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }
//-------------------------------------------------------------------------------------------//
    private void HandleStarting() {
        levelComplete = false;
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player is null");
        }
        // Do some start setup, could be environment, cinematics etc

        // reset stars GFX
        // foreach (GameObject obj in Stars)
        // {
        //     obj.SetActive(false);
        // }

        if (mustCreateVehicle)
        {
            playerControl = false;
            Time.timeScale = 0;
        }
        else 
        {
            playerControl = true;
        }
        gameOverPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        createVehiclePanel.SetActive(mustCreateVehicle);
        player = GameObject.Find("Player").GetComponent<Player>();
        // Eventually call ChangeState again with your next state
        
        ChangeState(GameState.SpawningHeroes);
    }
//-------------------------------------------------------------------------------------------//
    private void HandleSpawningHeroes() {
        // UnitManager.Instance.SpawnEnemies();
        
        ChangeState(GameState.SpawningEnemies);
    }
    private void HandleSpawningEnemies() {
        
        // Spawn enemies
        
        ChangeState(GameState.HeroTurn);
    }
//-------------------------------------------------------------------------------------------//
    private void HandleHeroTurn() {
        // If you're making a turn based game, this could show the turn menu, highlight available units etc
        
        // Keep track of how many units need to make a move, once they've all finished, change the state. This could
        // be monitored in the unit manager or the units themselves.
    }
//-------------------------------------------------------------------------------------------//
    public void GameOver()
    {
        Time.timeScale =0;
        isGameOver = true;
        
        gameOverPanel.SetActive(true);
        pausePanel.SetActive(true);
    }
//-------------------------------------------------------------------------------------------//
    public void LevelComplete()
    {
        // stars =1;
        // if (player.coins >= 100)
        // {
        //     stars+=1;
        // }
        // if (!player.tookDamage)
        // {
        //     stars +=1;
        // }
        // switch (stars)
        // {
        //     case 1:
        //         Stars[0].SetActive(true);
        //         break;
        //     case 2:
        //         Stars[0].SetActive(true);
        //         Stars[1].SetActive(true);
        //         break;
        //     case 3:
        //         Stars[0].SetActive(true);
        //         Stars[1].SetActive(true);
        //         Stars[2].SetActive(true);
        //         break;
        //     default:
        //         break;
        // }
        Time.timeScale =0;
        playerControl = false;
        isGameOver = true;
        levelComplete = true;
        levelCompletePanel.SetActive(true);
    }
//-------------------------------------------------------------------------------------------//
    public void Resume()
    {
        Invoke("EnablePlayerControl",0.1f);
        createVehiclePanel.SetActive(false);
        selectGadgetPanel.SetActive(false);
        pausePanel.SetActive(false);
        isPaused=false;
        Time.timeScale =1;
    }
    public void EnablePlayerControl()
    {
        playerControl = true;
    }
    public void DisablePlayerControl()
    {
        playerControl = false;
    }
//-------------------------------------------------------------------------------------------//
    public void LoadLevel(string level)
    {
        Time.timeScale =1;
        SceneManager.LoadScene(level);
    }

    public void LoadNextLevel()
    {
        Time.timeScale =1;
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel+1);
    }
//-------------------------------------------------------------------------------------------//
    public void Restart()
    {
        Time.timeScale =1;
        levelComplete = false;
        string currentScene = SceneManager.GetActiveScene().name;
        LoadLevel(currentScene);
    }

    public void Pause()
    {
        if (isPaused)
        {
            // resume game
            pausePanel.SetActive(false);
            playerControl = true;
            Time.timeScale =1;
            isPaused=false;
        }
        else 
        {
            // pause the game
            isPaused = true;
            playerControl = false;
            pausePanel.SetActive(true);
            Time.timeScale =0;
        }
    }
//-------------------------------------------------------------------------------------------//
    public void LoadMenu()
    {
        Time.timeScale =1;
        SceneManager.LoadScene(0);
    }
//-------------------------------------------------------------------------------------------//

    public void ExitGame()
    {
        Application.Quit();
    }
//-------------------------------------------------------------------------------------------//    
}
