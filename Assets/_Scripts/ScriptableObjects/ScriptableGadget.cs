using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gadgets", menuName = "ScriptableObjects/Gadgets")]
public class ScriptableGadget : ScriptableObject
{
    [SerializeField] private Stats _advancedStats;
    public Stats avancedStats => _advancedStats;
    
    [System.Serializable]
    public struct Stats
    {
        public float mass;
        public bool isPlayer;

    }
}
