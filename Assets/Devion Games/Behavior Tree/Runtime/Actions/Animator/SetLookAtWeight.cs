using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Set look at weights.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.SetLookAtWeight.html")]
	public class SetLookAtWeight: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("(0-1) the global weight of the LookAt, multiplier for other parameters.")]
		public FloatVariable weight;
		[Tooltip ("(0-1) determines how much the body is involved in the LookAt.")]
		public FloatVariable bodyWeight;
		[Tooltip ("(0-1) determines how much the head is involved in the LookAt.")]
		public FloatVariable headWeight = 1;
		[Tooltip ("(0-1) determines how much the eyes are involved in the LookAt.")]
		public FloatVariable eyesWeight;
		[Tooltip ("(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).")]
		public FloatVariable clampWeight = 0.5f;

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
			m_Animator.SetLookAtWeight (weight, bodyWeight, headWeight, eyesWeight, clampWeight);
			return TaskStatus.Success;
		}
	}
}
