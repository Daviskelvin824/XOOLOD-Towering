using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollStateMinion : StateMachineBehaviour
{
    public MinionData data;
    float timer;
    NavMeshAgent agent;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        agent = animator.GetComponent<NavMeshAgent>();
        player = PlayerManager.instance.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = data.walkSpeed * data.baseSpeed;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector2 randomDirection = Random.insideUnitCircle * data.walkRange;
            Vector3 randomDestination = animator.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);

            agent.SetDestination(randomDestination);
        }
        timer += Time.deltaTime;
        if (timer > data.walkCooldown)
        {
            animator.SetBool("isPatrolling", false);
        }

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < data.aggroRange)
        {
            animator.Play("Scream");
            animator.SetBool("isChasing", true);
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
