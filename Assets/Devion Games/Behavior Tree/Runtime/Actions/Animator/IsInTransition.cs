using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Is the specified AnimatorController layer in a transition.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.IsInTransition.html")]
	public class IsInTransition: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The layer's index.")]
		public IntVariable layerIndex;

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
			return m_Animator.IsInTransition (layerIndex) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
