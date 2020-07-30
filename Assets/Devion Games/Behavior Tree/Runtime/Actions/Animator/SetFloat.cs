using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Sets the float parameter value.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.SetFloat.html")]
	public class SetFloat: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public StringVariable m_name;
		public FloatVariable value;
		public FloatVariable m_DampTime;


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
			if (!m_DampTime.isNone && m_DampTime.Value > 0f)
			{
				m_Animator.SetFloat(m_name.Value, value, m_DampTime.Value, Time.deltaTime);
			}
			else
			{
				m_Animator.SetFloat(m_name.Value, value);
			}
			return TaskStatus.Success;
		}
	}
}
