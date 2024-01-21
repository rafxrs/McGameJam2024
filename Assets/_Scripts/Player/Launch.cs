using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public float launchForce = 10f;  // Adjust this value to control the launch force

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if a specific key is pressed, for example, the "Space" key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call the function or perform the actions you want to execute when the key is pressed
            YourFunctionToExecute();
        }
    }

    void YourFunctionToExecute()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Ensure the Rigidbody2D component is not null
        if (rb != null)
        {
            // Calculate the launch direction (you can modify this based on your needs)
            Vector2 launchDirection = Vector2.right;

            // Apply force to launch the object
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on the GameObject!");
        }
    }
}
