using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : StateMachineBehaviour
{

    NavMeshAgent navAgent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponentInParent<NavMeshAgent>();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ChaseState.ShouldChasePlayer(animator.transform.position))
        {
            animator.SetInteger(EnemyAIManager.Instance().transitionParameter, (int)Transition.CHASE);
        }
        else if (!navAgent.hasPath)
        {
            animator.SetInteger(EnemyAIManager.Instance().transitionParameter, (int)Transition.IDLE);
        }
    }
}
