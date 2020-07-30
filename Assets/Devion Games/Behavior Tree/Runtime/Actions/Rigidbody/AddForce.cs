using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("Adds a force to the Rigidbody.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html")]
	public class AddForce: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Force vector in world coordinates.")]
		public Vector3Variable force;
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
			m_Rigidbody.AddForce (force.Value, mode);
			return TaskStatus.Success;
		}
	}
}
