using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("The velocity of the rigidbody at the point worldPoint in global space.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.GetPointVelocity.html")]
	public class GetPointVelocity: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public Vector3Variable worldPoint;
		[Shared]
		public Vector3Variable m_PointVelocity;

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
			m_PointVelocity.Value = m_Rigidbody.GetPointVelocity (worldPoint);
			return TaskStatus.Success;
		}
	}
}
