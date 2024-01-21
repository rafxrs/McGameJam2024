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
        private UIManager _uiManager;

        public GameObject currentlySelectedGadget;
        public GameObject currentlySelectedSlot;
        public bool isRotateSelected;
        public bool isRotateRight;
        public int currentlySelectedGadgetNumber;

        private Rigidbody2D rb;

        public float mass = 0f;
        public float speed = 20f;
        public float airDrag = 0f;
        public float adherence;

        public string tag_bomb = "Bomb";
        
        float _horizontalInput;
        int _currentHealth=100;
        bool _jump;
        bool _isDead;
        public float launchForce = 1500f;  // Adjust this value to control the launch force


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

        private Vector2 launchDirection;


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
            _uiManager = GameObject.Find("Main Canvas").GetComponent<UIManager>();

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

            if (GameManager.playerControl) 
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Activate_Bombe();
                }
            }

        }




        void update_direction(string positionName)
        {
            Debug.Log("start" + launchDirection);
            Vector2 original_direction = Vector2.zero;
            switch (positionName)
            {  
                case "Top Left": original_direction = Vector2.right + Vector2.down; break;
                case "Top": original_direction = Vector2.down; break;
                case "Top Right": original_direction = Vector2.left + Vector2.down; break;
                case "Front": original_direction = Vector2.left; break;
                case "Middle": original_direction = Vector2.zero; break;
                case "Back": original_direction = Vector2.right; break;
                case "Bottom Left": original_direction = Vector2.right + Vector2.up; break;
                case "Bottom": original_direction = Vector2.up; break;
                case "Bottom Right": original_direction = Vector2.left + Vector2.up; break;
                default: original_direction = Vector2.zero; break;
            }
            // Une fois qu'on a récupéré la direction supposée on applique la rotation 


            Quaternion rotation = transform.rotation;
            float angleInDegrees = rotation.eulerAngles.z;
            // Convert the angle to radians
            float angleInRadians = Mathf.Deg2Rad * angleInDegrees;

            // Modify the Vector2 using the angle
            Vector2 modifiedVector = RotateVector(original_direction, angleInRadians);
            launchDirection += modifiedVector;
            Debug.Log("end" + launchDirection);
        }

        public static Vector2 RotateVector(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
}

        void Activate_Bombe()
        {
            launchDirection = Vector2.zero;
            if(top != null && top.CompareTag(tag_bomb) == true) { update_direction("Top") ; Destroy(top);} 
            if(topLeft != null && topLeft.CompareTag(tag_bomb) == true)  { update_direction("Top Left" ); Destroy(topLeft);} 
            if(topRight != null && topRight.CompareTag(tag_bomb) == true) { update_direction("Top Right" ); Destroy(topRight);} 
            if(front != null && front.CompareTag(tag_bomb) == true) { update_direction("Front"); Destroy(front);} 
            if(middle != null && middle.CompareTag(tag_bomb) == true) { update_direction("Middle" ); Destroy(middle);} 
            if(back != null && back.CompareTag(tag_bomb) == true)  { update_direction("Back"); Destroy(back);} 
            if(bottomRight != null && bottomRight.CompareTag(tag_bomb) == true) { update_direction("Bottom Right"); Destroy(bottomRight);} 
            if(bottomLeft != null && bottomLeft.CompareTag(tag_bomb) == true) { update_direction("Bottom Left"); Destroy(bottomLeft);} 
            if(bottom != null && bottom.CompareTag(tag_bomb) == true) { update_direction("Bottom"); Destroy(bottom);} 

            if (launchDirection != Vector2.zero) {
                Launch_Player();
            }
        }

        void Launch_Player()
        {
            if (rb != null)
            {
                // Apply force to launch the object
                rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
            }
            else {
                Debug.LogError("Rigidbody2D component not found on the GameObject!");
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
            if (other.gameObject.CompareTag("DeathFloor"))
            {
                TakeDamage(100);
            }
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("DeathFloor"))
            {
                TakeDamage(100);
            }  
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
            Invoke("GameOverSequence",0.35f);
            FindObjectOfType<AudioManager>().Play("Death1");
            _isDead = true;
            GetComponent<CharacterController2D>().enabled = false;
            Destroy(this.gameObject, 4f);
        }
        //-------------------------------------------------------------------------------------------//
        // UI AND SCORE FUNCTIONS
        //-------------------------------------------------------------------------------------------//
        
        void GameOverSequence()
        {
            _uiManager.GameOverSequence();
        }
        public void LevelComplete()
        {
            _uiManager.LevelComplete();
        }

        public void SelectGadget(int number)
        {
            
            currentlySelectedGadgetNumber = number;
            if (number == 10)
            {
                isRotateSelected = true;
                isRotateRight = true;
            }
            else if (number == 11)
            {
                isRotateSelected = true;
                isRotateRight = false;
            }
            else
            {
                currentlySelectedGadget = gadgets[number];
                isRotateSelected = false;
            }
            Debug.Log("Selected "+currentlySelectedGadget);
            FindObjectOfType<AudioManager>().Play("SelectGadgetSound");

        }

        public void SetGadget(string positionName)
        {
            // Must select gadget
            if (currentlySelectedGadget == null) {
                Debug.Log("il faut sélectionner un objet à mettre sur la position " + positionName);
                return;
            }
            // destroy children if not rotating
            if (!isRotateSelected)
            {
                if (transform.Find(positionName).childCount > 0) {
                    Destroy(transform.Find(positionName).GetChild(0).gameObject);
                } 
            }
            
            switch (currentlySelectedGadgetNumber)
            {
                case 0:
                    FindObjectOfType<AudioManager>().Play("WoodSound");
                    break;
                case 1:
                    FindObjectOfType<AudioManager>().Play("WoodSound");
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
                case 5:
                    FindObjectOfType<AudioManager>().Play("WoodSound");
                    break;
                case 6:
                    FindObjectOfType<AudioManager>().Play("WoodSound");
                    break;
                case 7:
                    FindObjectOfType<AudioManager>().Play("SpringSound");
                    break;
                case 8:
                    FindObjectOfType<AudioManager>().Play("TntSound");
                    break;
            }
            
            ScriptableGadget currentScriptableGadget =
                currentlySelectedGadget.GetComponent<Gadget>().gadgetScriptableObject;
            
            switch (positionName)
            {  
                case "Top Left":
                    if (!isRotateSelected)
                    {
                        if (topLeft != null)
                        {
                            Destroy(topLeft);
                        }
                        instantiateAt = topLeftPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            topLeft = Instantiate(currentlySelectedGadget,instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            topLeft =Instantiate(currentlySelectedGadget,instantiatePos, Quaternion.identity);
                            Set_Linked(topLeft,positionName);
                            topLeft.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) topLeft.transform.Rotate(new Vector3(0,0,90));
                        else topLeft.transform.Rotate(new Vector3(0,0,-90));
                    }
                    
                    break;
                case "Top":
                    if (!isRotateSelected)
                    {
                        if (top != null)
                        {
                            Destroy(top);
                        }

                        instantiateAt = topPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            top = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            top = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(top, positionName);
                            top.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) top.transform.Rotate(new Vector3(0,0,90));
                        else top.transform.Rotate(new Vector3(0,0,-90));
                    }

                    break;
                case "Top Right":
                    if (!isRotateSelected)
                    {
                        if (topRight != null)
                        {
                            Destroy(topRight);
                        }

                        instantiateAt = topRightPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            topRight = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            topRight = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(topRight, positionName);
                            topRight.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) topRight.transform.Rotate(new Vector3(0,0,90));
                        else topRight.transform.Rotate(new Vector3(0,0,-90));
                    }

                    break;
                case "Front":
                    if (!isRotateSelected)
                    {
                        if (front != null)
                        {
                            Destroy(front);
                        }

                        instantiateAt = frontPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            front = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            front = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(front, positionName);
                            front.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) front.transform.Rotate(new Vector3(0,0,90));
                        else front.transform.Rotate(new Vector3(0,0,-90));
                    }

                    break;
                case "Middle":
                    if (!isRotateSelected)
                    {
                        if (middle != null)
                        {
                            Destroy(middle);
                        }

                        instantiateAt = middlePos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            middle = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            middle = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(middle, positionName);
                            middle.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) middle.transform.Rotate(new Vector3(0,0,90));
                        else middle.transform.Rotate(new Vector3(0,0,-90));
                    }
                    break;
                case "Back":
                    if (!isRotateSelected)
                    {
                        if (back != null)
                        {
                            Destroy(back);
                        }

                        instantiateAt = backPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            back = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            back = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(back, positionName);
                            back.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) back.transform.Rotate(new Vector3(0,0,90));
                        else back.transform.Rotate(new Vector3(0,0,-90));
                    }
                    break;
                case "Bottom Left":
                    if (!isRotateSelected)
                    {
                        if (bottomLeft != null)
                        {
                            Destroy(bottomLeft);
                        }

                        instantiateAt = bottomLeftPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            bottomLeft = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            bottomLeft = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(bottomLeft, positionName);
                            bottomLeft.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) bottomLeft.transform.Rotate(new Vector3(0,0,90));
                        else bottomLeft.transform.Rotate(new Vector3(0,0,-90));
                    }
                    break;
                case "Bottom":
                    if (!isRotateSelected)
                    {
                        if (bottom != null)
                        {
                            Destroy(bottom);
                        }

                        instantiateAt = bottomPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            bottom = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            bottom = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(bottom, positionName);
                            bottom.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) bottom.transform.Rotate(new Vector3(0,0,90));
                        else bottom.transform.Rotate(new Vector3(0,0,-90));
                    }
                    break;
                case "Bottom Right":
                    if (!isRotateSelected)
                    {
                        if (bottomRight != null)
                        {
                            Destroy(bottomRight);
                        }

                        instantiateAt = bottomRightPos;
                        instantiatePos = instantiateAt.transform.position;
                        if (currentScriptableGadget.avancedStats.isPlayer)
                        {
                            bottomRight = Instantiate(currentlySelectedGadget, instantiateAt);
                            rb.mass += currentScriptableGadget.avancedStats.mass;
                        }
                        else
                        {
                            bottomRight = Instantiate(currentlySelectedGadget, instantiatePos, Quaternion.identity);
                            Set_Linked(bottomRight, positionName);
                            bottomRight.GetComponent<Rigidbody2D>().mass = currentScriptableGadget.avancedStats.mass;
                        }
                    }
                    else
                    {
                        if (isRotateRight) bottomRight.transform.Rotate(new Vector3(0,0,90));
                        else bottomRight.transform.Rotate(new Vector3(0,0,-90));
                    }
                    break;
                
            }  
        } 

        void Set_Linked(GameObject object_created, string positionName)
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

            if (object_created.name.Contains("BigWheel")) 
            {
                WheelJoint2D wheelJoint = object_created_rb.AddComponent<WheelJoint2D>();
                wheelJoint.connectedBody = rb;
                
                Vector2 original_direction = Vector2.zero;
                switch (positionName)
                {  
                    case "Top Left": original_direction = Vector2.left + Vector2.up; break;
                    case "Top": original_direction = Vector2.up; break;
                    case "Top Right": original_direction = Vector2.right + Vector2.up; break;
                    case "Front": original_direction = Vector2.right; break;
                    case "Middle": original_direction = Vector2.zero; break;
                    case "Back": original_direction = Vector2.left; break;
                    case "Bottom Left": original_direction = Vector2.left + Vector2.down; break;
                    case "Bottom": original_direction = Vector2.down; break;
                    case "Bottom Right": original_direction = Vector2.right + Vector2.down; break;
                    default: original_direction = Vector2.zero; break;
                }
                // Set the updated connectedAnchor
                wheelJoint.connectedAnchor = original_direction;

                DistanceJoint2D distanceJoint = object_created_rb.AddComponent<DistanceJoint2D>();
                distanceJoint.connectedBody = rb;
        
                // Set the maximum distance for the DistanceJoint2D based on the original direction
                distanceJoint.distance = original_direction.magnitude * 0.8f;

            } 
            else {
                FixedJoint2D fixedJoint = object_created_rb.AddComponent<FixedJoint2D>();
                fixedJoint.connectedBody = rb;
            }
        }

        public void Rotate90()
        {
            
        }
    }
}
