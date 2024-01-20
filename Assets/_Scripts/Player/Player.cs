using System;
using UnityEngine;

namespace _Scripts.Units.Player
{
    /// <summary>
    /// Main class for the player
    /// </summary>

//-------------------------------------------------------------------------------------------//
    public class Player : MonoBehaviour
    {
        public LayerMask platformLayerMask;
        public CharacterController2D _characterController;
        public GameObject[] gadgets;

        public GameObject currentlySelectedGadget;
        
        public float speed = 20f;
        float _horizontalInput;
        int _currentHealth;
        bool _jump;
        bool _isDead;

        private Transform top;
        private Transform topLeft;
        private Transform topRight;
        private Transform front;
        private Transform back;
        private Transform bottomRight;
        private Transform bottomLeft;
        private Transform bottom;

        //-------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------//
        // START
        void Start()
        {
            top = transform.Find("Top");
            topLeft = transform.Find("Top Left");
            topRight = transform.Find("Top Right");
            front = transform.Find("Front");
            back = transform.Find("Back");
            bottomLeft = transform.Find("Bottom Left");
            bottomRight = transform.Find("Bottom Right");
            bottom = transform.Find("Bottom");
        }

        //-------------------------------------------------------------------------------------------//
        // UPDATE
        void Update()
        {
            if (!_isDead)
            {
                _horizontalInput = Input.GetAxisRaw("Horizontal") *  speed;
                
                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) )
                {
                    Jump();
                }
            }
            else 
            {
                _horizontalInput =0f;
            }
        
        
        }

        void FixedUpdate()
        {
            // if (GameManager.playerControl)
            // {
                _characterController.Move(_horizontalInput* Time.fixedDeltaTime, false, _jump);
                _jump = false;
            // }
        

        }

        //-------------------------------------------------------------------------------------------//

        /*
    MOVEMENT FUNCTIONS
    */
        public void OnLanding()
        {
        }
        public bool isGrounded()
        {
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            bool grounded = collider.IsTouchingLayers(platformLayerMask);
            return grounded;
        }
        public void Jump()
        {
            _jump = true;
        }

        //-------------------------------------------------------------------------------------------//
        
        void OnCollisionEnter2D(Collision2D other)
        {
            // if (other.gameObject.CompareTag("DeathFloor"))
            // {
            //     TakeDamage(_playerScriptable.baseStats.maxHealth);
            // }
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            
        }

        void OnTriggerExit2D(Collider2D other)
        {

        }
        

        //-------------------------------------------------------------------------------------------//

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        
        void Die()
        {
            Invoke("GameOverSequence",1.5f);
            _isDead = true;
            GetComponent<CharacterController2D>().enabled = false;
            Destroy(this.gameObject, 4f);
        }
        //-------------------------------------------------------------------------------------------//
        // UI AND SCORE FUNCTIONS
        //-------------------------------------------------------------------------------------------//
        
        // void GameOverSequence()
        // {
        //     _uiManager.GameOverSequence();
        // }
        // public void LevelComplete()
        // {
        //     _uiManager.LevelComplete();
        // }

        public void SelectGadget(int number)
        {

            currentlySelectedGadget = gadgets[number];
            Debug.Log("Selected "+currentlySelectedGadget);
        }

        public void SetGadget(string positionName)
        {
            if (transform.Find(positionName).childCount > 0) {
                Destroy(transform.Find(positionName).GetChild(0).gameObject);
            }

            if (currentlySelectedGadget == null) {
                Debug.Log("il faut sélectionner un objet à mettre sur la position " + positionName);
                return;
            }

            switch (positionName)
            {
                case "Top Left":
                    Instantiate(currentlySelectedGadget,topLeft);
                    break;
                case "Top":
                    Instantiate(currentlySelectedGadget,top);
                    break;
                case "Top Right":
                    Instantiate(currentlySelectedGadget,topRight);
                    break;
                case "Front":
                    Instantiate(currentlySelectedGadget,front);
                    break;
                case "Back":
                    Instantiate(currentlySelectedGadget,back);
                    break;
                case "Bottom Left":
                    Instantiate(currentlySelectedGadget,bottomLeft);
                    break;
                case "Bottom":
                    Instantiate(currentlySelectedGadget,bottom);
                    break;
                case "Bottom Right":
                    Instantiate(currentlySelectedGadget,bottomRight);
                    break;
            }
        }
        
    }
}
