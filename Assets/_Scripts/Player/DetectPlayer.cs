using System.Collections;
using System.Collections.Generic;
using _Scripts.Units.Player;
using UnityEngine;
public class DetectPlayer : MonoBehaviour
{
    public bool playerDetected;
    public Action action;
    public GameObject doActionOn;
    GameObject player;
    Transform playerTransform;
    GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerTransform = player.GetComponent<Transform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            // do action
            switch (action)
            {
                case Action.EndOfLevel:
                    Debug.Log("Finishing level");
                    gameManager.LevelComplete(); 
                    // Destroy(this.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
    [System.Serializable]
    public enum Action
    {
        Tutorial,
        EndOfLevel,
        Lever,
        Teleport,

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerDetected = true;
            FindObjectOfType<AudioManager>().Play("VictorySound");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Debug.Log("Player left the collider");
            playerDetected = false;
            FindObjectOfType<AudioManager>().Play("VictorySound");
        }
    }
}
