using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateMonitor : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (stateInfo.IsTag("Starto"))
    //    {
    //        animator.SetBool("IsAttacking", true);
    //        Debug.Log("start attack");
    //    }
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (stateInfo.IsTag("Exito"))
    //    {
    //        animator.SetBool("IsAttacking", false);
    //        Debug.Log("stop attack");
    //    }
    //}

    //OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool("IsAttacking", true);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
         animator.SetBool("IsAttacking", false);

        //reset triggers
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(p.name);
        }
        //for (int i = 0; i <= animator.parameterCount; i++)
        //{
        //    //check if it's a trigger type parameters
        //    if (animator.GetParameter(i).GetType() == animator.GetParameter(4).GetType())
        //    {
        //        animator.ResetTrigger(i);
        //    }
        //}
    }
}
