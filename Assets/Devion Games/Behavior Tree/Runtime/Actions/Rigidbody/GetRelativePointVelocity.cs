using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("The velocity relative to the rigidbody at the point relativePoint.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.GetRelativePointVelocity.html")]
	public class GetRelativePointVelocity: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public Vector3Variable relativePoint;
		[Shared]
		public Vector3Variable m_RelativePointVelocity;

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
			m_RelativePointVelocity.Value = m_Rigidbody.GetRelativePointVelocity (relativePoint);
			return TaskStatus.Success;
		}
	}
}
