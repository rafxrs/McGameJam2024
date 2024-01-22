using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Units.Player;
using UnityEngine;

public class Gadget : MonoBehaviour
{
    public ScriptableGadget gadgetScriptableObject;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("DeathFloor"))
        {
            GameObject.Find("Player").GetComponent<Player>().TakeDamage(100);
        }
    }
}
