using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DragonData 
{
    public float baseSpeed = 5f;
    public float rotationSpeed = 0.05f;
    public float walkSpeed = 0.225f;
    public float walkRange = 7.5f;
    public float walkCooldown = 3f;
    public float aggroRange = 12.5f;
    public float chaseSpeed = 1.1f;
    public float chaseCooldown = 0.5f;
    public float generalAttackCooldown = 2f;
    public float takeDamageCooldown = 0.5f;
    public float damageMultiplier = 1f;
    public float hitCount = 1f;
    public float attackRange = 8f;
    public float attackCooldown = 2.5f;

    public float damageMultiSpawn = 0f;
    public float hitCountSpawn = 0;
    public float attackRangeSpawn = 20f;
    public float attackCooldownSpawn = 20f;

}