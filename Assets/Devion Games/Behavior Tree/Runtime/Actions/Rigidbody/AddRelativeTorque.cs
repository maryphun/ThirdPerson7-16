using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("Adds a torque to the rigidbody relative to its coordinate system.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.AddRelativeTorque.html")]
	public class AddRelativeTorque: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Torque vector in local coordinates.")]
		public Vector3Variable torque;
		[Tooltip ("Type of force to apply.")]
		public ForceMode mode;

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
			m_Rigidbody.AddRelativeTorque (torque, mode);
			return TaskStatus.Success;
		}
	}
}
