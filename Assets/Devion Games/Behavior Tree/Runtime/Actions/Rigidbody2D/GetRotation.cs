using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("The rotation of the rigidbody.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D-rotation.html")]
	public class GetRotation: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_Rotation;

		private GameObject m_PrevGameObject;
		private Rigidbody2D m_Rigidbody2D;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Rigidbody2D = m_gameObject.Value.GetComponent<Rigidbody2D> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Rigidbody2D == null) {
				Debug.LogWarning ("Missing Component of type Rigidbody2D!");
				return TaskStatus.Failure;
			}
			m_Rotation.Value = m_Rigidbody2D.rotation;
			return TaskStatus.Success;
		}
	}
}
