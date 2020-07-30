using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("Moves the rigidbody to position.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.MovePosition.html")]
	public class MovePosition: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The new position for the Rigidbody object.")]
		public Vector3Variable m_position;

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
			m_Rigidbody.MovePosition (m_position);
			return TaskStatus.Success;
		}
	}
}
