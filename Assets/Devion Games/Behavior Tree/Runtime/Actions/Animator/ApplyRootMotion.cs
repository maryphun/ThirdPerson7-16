using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Should root motion be applied?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator-applyRootMotion.html")]
	public class ApplyRootMotion: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

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
			return m_Animator.applyRootMotion ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
