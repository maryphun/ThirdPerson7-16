using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("Is the rigidbody sleeping?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.IsSleeping.html")]
	public class IsSleeping: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		private GameObject m_PrevGameObject;
		private Rigidbody m_Rigidbody;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Rigidbody = m_gameObject.Value.GetComponent<Rigidbody> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Rigidbody == null) {
				Debug.LogWarning ("Missing Component of type Rigidbody!");
				return TaskStatus.Failure;
			}
			return m_Rigidbody.IsSleeping () ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
