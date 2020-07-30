using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Sets a trigger parameter to active. A trigger parameter is a bool parameter that gets reset to false when it has been used in a transition. For state machines with multiple layers, the trigger will only get reset once all layers have been evaluated, so that the layers can synchronize their transitions on the same parameter.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.SetTrigger.html")]
	public class SetTrigger: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public StringVariable m_name;

		private GameObject m_PrevGameObject;
		private Animator m_Animator;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Animator = m_gameObject.Value.GetComponent<Animator> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Animator == null) {
				Debug.LogWarning ("Missing Component of type Animator!");
				return TaskStatus.Failure;
			}
			m_Animator.SetTrigger (m_name.Value);
			return TaskStatus.Success;
		}
	}
}
