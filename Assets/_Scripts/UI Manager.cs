using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject containers;
    GameManager gameManager;
    public int coins;
    // Start is called before the first frame update
    void Start()
    {        
        containers = GameObject.Find("Containers");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelComplete()
    {
        gameManager.LevelComplete();
    }
    public void GameOverSequence()
    {
        gameManager.GameOver();
        // StartCoroutine(GameOverRoutine());

    }
}