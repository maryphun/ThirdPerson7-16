using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class IdleState : StateMachineBehaviour
{

    NavMeshAgent navAgent;
    public float minWanderDistance = 5;
    public float maxWanderDistance = 15;
    public float walkspeed = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponentInParent<NavMeshAgent>();
        }

        navAgent.ResetPath();
        navAgent.speed = walkspeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ChaseState.ShouldChasePlayer(animator.transform.position))
        {
            animator.SetInteger(EnemyAIManager.Instance().transitionParameter, (int)Transition.CHASE);
        }
        else
        {
            if (navAgent.hasPath)
            {
                animator.SetInteger(EnemyAIManager.Instance().transitionParameter, (int)Transition.WANDER);
            }
            else
            {
                SetRandomDestination(navAgent);
            }
        }
    }

    public void SetRandomDestination(NavMeshAgent agent)
    {
        float radius = Random.Range(minWanderDistance, maxWanderDistance);
        Vector3 randomPosition = Random.insideUnitSphere * radius;

        randomPosition += agent.transform.position;
        NavMeshHit hit;

        // determine if the randomed destination is in the navmesh. 
        //If it's not valid and the destination will remain null and this function will be called again to search.
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            agent.SetDestination(hit.position);
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
