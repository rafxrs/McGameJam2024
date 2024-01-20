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
        private CharacterController2D _characterController;
        
        public float speed = 20f;
        float _horizontalInput;
        int _currentHealth;
        bool _jump;
        bool _isDead;

        //-------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------//
        // START
        void Start()
        {
            _characterController = GetComponent<CharacterController2D>();
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
        public void OnRoll()
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
        
        //-------------------------------------------------------------------------------------------//
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
        
    }
}
