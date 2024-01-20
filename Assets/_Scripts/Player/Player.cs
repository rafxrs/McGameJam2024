using System;
using Unity.VisualScripting;
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
        public int currentlySelectedGadgetNumber;

        private Rigidbody2D rb;

        public float mass = 0f;
        public float speed = 20f;
        public float airDrag = 0f;
        public float adherence;
        
        float _horizontalInput;
        int _currentHealth;
        bool _jump;
        bool _isDead;

        private Transform topPos;
        private Transform topLeftPos;
        private Transform topRightPos;
        private Transform frontPos;
        private Transform middlePos;
        private Transform backPos;
        private Transform bottomRightPos;
        private Transform bottomLeftPos;
        private Transform bottomPos;
        
        private GameObject top;
        private GameObject topLeft;
        private GameObject topRight;
        private GameObject front;
        private GameObject middle;
        private GameObject back;
        private GameObject bottomRight;
        private GameObject bottomLeft;
        private GameObject bottom;
        
        Transform instantiateAt;
        private Vector3 instantiatePos;

        //-------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------//
        // START
        void Start()
        {
            topPos = transform.Find("Top");
            topLeftPos = transform.Find("Top Left");
            topRightPos = transform.Find("Top Right");
            frontPos = transform.Find("Front");
            middlePos = transform.Find("Middle");
            backPos = transform.Find("Back");
            bottomLeftPos = transform.Find("Bottom Left");
            bottomRightPos = transform.Find("Bottom Right");
            bottomPos = transform.Find("Bottom");

            rb = GetComponent<Rigidbody2D>();
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
            currentlySelectedGadgetNumber = number;
            Debug.Log("Selected "+currentlySelectedGadget);
            //TODO
            // audio manager plays a select sound
            FindObjectOfType<AudioManager>().Play("SelectGadgetSound");

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

            switch (currentlySelectedGadgetNumber)
            {
                case 0:
                    FindObjectOfType<AudioManager>().Play("WoodSound");
                    break;
                case 1:
                    FindObjectOfType<AudioManager>().Play("MotorSound");
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Play("BalloonSound");
                    break;
                case 3:
                    FindObjectOfType<AudioManager>().Play("UmbrellaSound");
                    break;
                case 4:
                    FindObjectOfType<AudioManager>().Play("WheelSound");
                    break;
            }
            
            switch (positionName)
            {  
                case "Top Left":
                    if (topLeft != null)
                    {
                        Destroy(topLeft);
                    }
                    instantiateAt = topLeftPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        topLeft = Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        topLeft =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(topLeft);
                    }
                    break;
                case "Top":
                    if (top != null)
                    {
                        Destroy(top);
                    }
                    instantiateAt = topPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        top =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        top =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(top);
                    }
                    break;
                case "Top Right":
                    if (topRight != null)
                    {
                        Destroy(topRight);
                    }
                    instantiateAt = topRightPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        topRight =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        topRight =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(topRight);
                    }
                    break;
                case "Front":
                    if (front != null)
                    {
                        Destroy(front);
                    }
                    instantiateAt = frontPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        front =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        front =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(front);
                    }
                    break;
                case "Middle":
                    if (middle != null)
                    {
                        Destroy(middle);
                    }
                    instantiateAt = middlePos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        middle =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        middle =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(middle);
                    }
                    break;
                case "Back":
                    if (back != null)
                    {
                        Destroy(back);
                    }
                    instantiateAt = backPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        back =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        back =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(back);
                    }
                    break;
                case "Bottom Left":
                    if (bottomLeft != null)
                    {
                        Destroy(bottomLeft);
                    }
                    instantiateAt = bottomLeftPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        bottomLeft =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        bottomLeft =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(bottomLeft);
                    }
                    break;
                case "Bottom":
                    if (bottom != null)
                    {
                        Destroy(bottom);
                    }
                    instantiateAt = bottomPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        bottom =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        bottom =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(bottom);
                    }
                    break;
                case "Bottom Right":
                    if (bottomRight != null)
                    {
                        Destroy(bottomRight);
                    }
                    instantiateAt = bottomRightPos;
                    instantiatePos = instantiateAt.transform.position;
                    if (currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject.avancedStats.isPlayer)
                    {
                        bottomRight =Instantiate(currentlySelectedGadget,instantiateAt);
                    }
                    else
                    {
                        bottomRight = Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                        Set_Linked(bottomRight);
                    }
                    break;
            }  
        } 

        void Set_Linked(GameObject object_created)
        {
            if (object_created == null) {
                Debug.LogError("Set_Linked : object_created shouldn't be null");
                return;
            }
            Rigidbody2D object_created_rb = object_created.GetComponent<Rigidbody2D>();
            if (object_created_rb == null) {
                Debug.LogError("Set_Linked : Rigidbody2D components not found on GameObjects!");
                return;
            }

            FixedJoint2D fixedJoint = object_created_rb.AddComponent<FixedJoint2D>();
            // Connect the FixedJoint2D to the second GameObject's Rigidbody2D
            fixedJoint.connectedBody = rb;
        } 
    }
}
