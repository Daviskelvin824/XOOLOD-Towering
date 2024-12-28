using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackStateDragon : StateMachineBehaviour
{
    public DragonData data;
    Transform player;
    float attackTimer = 0f;
    CharacterStats characterStats;
    public GameObject minionPrefab;
    float spawnTimer = 20f;
    bool isSpawning = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = PlayerManager.instance.transform;
        characterStats = CharacterStats.Instance.GetComponent<CharacterStats>();
        attackTimer = data.attackCooldown;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var newPlayerPos = player.position;
        newPlayerPos.y = animator.transform.position.y;
        animator.transform.LookAt(newPlayerPos);
        attackTimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            animator.SetTrigger("spawn");
            
            Instantiate(minionPrefab, new Vector3(animator.transform.position.x + 5, animator.transform.position.y, animator.transform.position.z + 5), Quaternion.identity);
            Instantiate(minionPrefab, new Vector3(animator.transform.position.x - 5, animator.transform.position.y, animator.transform.position.z + 5), Quaternion.identity);
            spawnTimer = 20f; // Reset spawn timer
        }

        if (attackTimer <= 0f)
        {
            // Reset the attack timer
            attackTimer = data.attackCooldown;

            // Trigger the attack action here
            // For example, you could call a method on the character's script to deal damage to the player
            animator.SetBool("isAttacking", false);
            if (characterStats != null)
            {
                characterStats.TakeDamage(10);
            }
        }
        else
        {
            // Set isAttacking to true while still in cooldown
            animator.SetBool("isAttacking", true);
        }
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance > data.attackRange)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
