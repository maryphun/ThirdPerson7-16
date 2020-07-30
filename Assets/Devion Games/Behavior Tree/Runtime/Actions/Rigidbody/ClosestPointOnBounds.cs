using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("The closest point to the bounding box of the attached colliders.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.ClosestPointOnBounds.html")]
	public class ClosestPointOnBounds: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public Vector3Variable m_position;
		[Shared]
		public Vector3Variable m_ClosestPointOnBounds;

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
			m_ClosestPointOnBounds.Value = m_Rigidbody.ClosestPointOnBounds (m_position);
			return TaskStatus.Success;
		}
	}
}
