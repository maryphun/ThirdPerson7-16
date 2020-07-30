using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("The center of mass relative to the transform's origin.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody-centerOfMass.html")]
	public class SetCenterOfMass: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public Vector3Variable m_CenterOfMass;

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
			m_Rigidbody.centerOfMass = m_CenterOfMass.Value;
			return TaskStatus.Success;
		}
	}
}
