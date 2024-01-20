using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gadgets", menuName = "ScriptableObjects/Gadgets")]
public class ScriptableGadget : ScriptableObject
{
    public GadgetType gadgetType;
    public LayerMask playerLayer;

    [SerializeField] private Stats _advancedStats;
    public Stats avancedStats => _advancedStats;

    [System.Serializable]
    public enum GadgetType {
        Wood,
        Umbrella,
        Tire,
        Rocket,
        Wheel,
    }

    /// <summary>
    /// Keeping base stats as a struct on the scriptable keeps it flexible and easily editable.
    /// We can pass this struct to the spawned prefab unit and alter them depending on conditions.
    /// </summary>
    [System.Serializable]
    public struct Stats
    {
        public float mass;
        public bool isPlayer;
        public float airDrag;
        public float reduceGravity;
        public float jumpForce;
    }
}
