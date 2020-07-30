using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("Controls whether physics affects the rigidbody.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody-isKinematic.html")]
	public class IsKinematic: Conditional
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
			return m_Rigidbody.isKinematic ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
