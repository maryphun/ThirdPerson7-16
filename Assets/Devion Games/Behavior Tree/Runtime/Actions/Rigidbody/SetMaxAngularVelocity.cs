using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("The maximimum angular velocity of the rigidbody. (Default 7) range { 0, infinity }.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody-maxAngularVelocity.html")]
	public class SetMaxAngularVelocity: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public FloatVariable m_MaxAngularVelocity;

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
			m_Rigidbody.maxAngularVelocity = m_MaxAngularVelocity.Value;
			return TaskStatus.Success;
		}
	}
}
