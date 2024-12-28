using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class PlayerData
{
    [field: SerializeField] public float baseSpeed = 5f;
    [field: SerializeField] public float walkSpeed = 0.4f;
    [field: SerializeField] public float nonCombatRunSpeed = 1.25f;
    [field: SerializeField] public float combatRunSpeed = 1.4f;
    [field: SerializeField] public float dashSpeed = 20f;
    [field: SerializeField] public float dashCooldownSeconds = 1.75f;
    [field: SerializeField] public float rollSpeed = 1f;
    [field: SerializeField] public float hardLandThreshold = 3f;
    [field: SerializeField] public float rotateSpeed = 0.1f;

}
