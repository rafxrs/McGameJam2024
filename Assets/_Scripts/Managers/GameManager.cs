using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using _Scripts.Units.Player;

public class GameManager : StaticInstance<GameManager>
{
//-------------------------------------------------------------------------------------------//
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public bool isGameFinished;

    public bool isGameOver;
    public bool isPaused;
    public bool levelComplete;
    public bool mustCreateVehicle;
    public static bool PlayerControl;

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject createVehiclePanel;
    public GameObject selectGadgetPanel;
    public GameObject levelCompletePanel;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        // p for pause
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) 
        {
            Pause();
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
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

        if (mustCreateVehicle)
        {
            PlayerControl = false;
            Time.timeScale = 0;
        }
        else 
        {
            PlayerControl = true;
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
        ChangeState(GameState.SpawningEnemies);
    }
    private void HandleSpawningEnemies() {
        ChangeState(GameState.HeroTurn);
    }
//-------------------------------------------------------------------------------------------//
    private void HandleHeroTurn() {

    }
//-------------------------------------------------------------------------------------------//
    public void GameOver()
    {
        Time.timeScale =0;
        isGameOver = true;
        gameOverPanel.SetActive(true);
        pausePanel.SetActive(true);
        // make resume button invisible when dying
        pausePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }
//-------------------------------------------------------------------------------------------//
    public void LevelComplete()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel + 1 == 7) LoadNextLevel();
        Time.timeScale =0;
        PlayerControl = false;
        isGameOver = true;
        levelComplete = true;
        if (!isGameFinished) levelCompletePanel.SetActive(true);
    }
//-------------------------------------------------------------------------------------------//
    public void Resume()
    {
        Invoke("EnablePlayerControl",0.1f);
        createVehiclePanel.SetActive(false);
        selectGadgetPanel.SetActive(false);
        pausePanel.SetActive(false);
        pausePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        isPaused=false;
        Time.timeScale =1;
    }
    public void EnablePlayerControl()
    {
        PlayerControl = true;
    }
    public void DisablePlayerControl()
    {
        PlayerControl = false;
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
            PlayerControl = true;
            Time.timeScale =1;
            isPaused=false;
        }
        else 
        {
            // pause the game
            isPaused = true;
            PlayerControl = false;
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
