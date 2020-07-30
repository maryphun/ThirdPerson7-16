using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityAnimator.UnityAnimatorStateInfo
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Does name match the name of the active state in the statemachine? The name should be in the form Layer.Name, for example \"Base.Idle\".")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/AnimatorStateInfo.IsName.html")]
	public class IsName : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public IntVariable layerIndex;
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
			AnimatorStateInfo currentAnimatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo (layerIndex);
			return currentAnimatorStateInfo.IsName (m_name.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}