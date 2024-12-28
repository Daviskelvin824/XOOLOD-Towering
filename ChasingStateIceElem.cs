using UnityEngine.AI;
using UnityEngine;

public class ChasingStateIceElem : StateMachineBehaviour
{
    public IceElementalData data;
    NavMeshAgent agent;
    Transform player;
    public CharacterStats characterStats;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = PlayerManager.instance.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = data.chaseSpeed * data.baseSpeed;
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance > data.aggroRange)
        {
            // Transition back to patrolling state
            Debug.Log("Get out of chasing");
            animator.SetBool("isChasing", false);
        }

        // If within attack range and not on cooldown, attack
        if (distance < data.attackRange)
        {
            animator.SetBool("isAttacking", true);

        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
