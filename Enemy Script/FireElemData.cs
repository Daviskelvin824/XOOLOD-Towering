using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElemData : MonoBehaviour
{
    public float baseSpeed = 4f;
    public float rotationSpeed = 0.05f;
    public float walkSpeed = 0.35f;
    public float walkRange = 10;
    public float walkCooldown = 3f;
    public float aggroRange = 15f;
    public float chaseSpeed = 0.75f;
    public float chaseCooldown = 0.5f;
    public float generalAttackCooldown = 2f;
    public float takeDamageCooldown = 0.5f;
    public float damageMultiplier = 1f;
    public float hitCount = 1f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
}
