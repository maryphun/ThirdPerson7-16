using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    public float repathTolerance = 2;
    public float repeatCount = 10;
    public float runSpeed = 4;
    public static float chaseRadius = 10;

    NavMeshAgent navAgent;
    ThirdPersonControl player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponentInParent<NavMeshAgent>();
        }

        if (player == null)
        {
            player = EnemyAIManager.Instance().player;
        }

        // Reset the path so the AI could create new path to chase the player
        navAgent.ResetPath();
        navAgent.speed = runSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!ShouldChasePlayer(animator.transform.position))
        {
            animator.SetInteger(EnemyAIManager.Instance().transitionParameter, (int)Transition.IDLE);
        }
        else
        {
            if (!navAgent.hasPath || (player.transform.position - navAgent.pathEndPosition).sqrMagnitude > repathTolerance * repathTolerance)
            {
                SetDestinationNearTarget(navAgent, player);
            }
        }
    }

    public void SetDestinationNearTarget(NavMeshAgent agent, ThirdPersonControl target)
    {
        NavMeshHit hit;
        float radius = 0;
        for (int i = 0; i < repeatCount; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * radius;
            randomPosition += target.transform.position;
            if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
            {
                agent.SetDestination(hit.position);
                break;
            }
            else
            {
                ++radius;
            }
        }
    }

    public static bool ShouldChasePlayer(Vector3 chaserPosition)
    {
        ThirdPersonControl p = EnemyAIManager.Instance().player;
        return ((p.transform.position - chaserPosition).sqrMagnitude < chaseRadius * chaseRadius);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

}
