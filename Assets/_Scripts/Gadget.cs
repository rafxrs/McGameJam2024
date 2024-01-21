using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Units.Player;
using UnityEngine;

public class Gadget : MonoBehaviour
{
    public ScriptableGadget gadgetScriptableObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // if (CompareTag("Balloon"))
        // {
        //     GameObject otherGameObject = other.gameObject;
        //     FixedJoint2D fixedjoint = GetComponent<FixedJoint2D>();
        //     Destroy(fixedjoint);
        //
        // }
        if (other.gameObject.tag.Equals("DeathFloor"))
        {
            GameObject.Find("Player").GetComponent<Player>().TakeDamage(100);
        }
    }
}
