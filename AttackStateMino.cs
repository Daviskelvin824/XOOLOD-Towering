using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class AttackStateMino : StateMachineBehaviour
{
    public MinotaurData data;
    Transform player;
    float attackTimer = 0f;
    CharacterStats characterStats;
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
