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
        // playerDetected = Physics2D.OverlapBox(areaPos.position, new Vector2(areaWidth, areaHeight),0,whatIsPlayer);

        if (playerDetected)
        {
            // do action
            switch (action)
            {
                case Action.EndOfLevel:
                    gameManager.ShowEButton();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("Finishing level");
                        gameManager.LevelComplete(); 
                        Destroy(this.gameObject);
                    }
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
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerDetected = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Debug.Log("Player left the collider");
            playerDetected = false;
            gameManager.HideEButton();
        }
    }
}
