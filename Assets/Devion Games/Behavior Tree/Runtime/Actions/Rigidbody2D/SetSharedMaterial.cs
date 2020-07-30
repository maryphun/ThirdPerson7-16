using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("The PhysicsMaterial2D that is applied to all Collider2D attached to this Rigidbody2D.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D-sharedMaterial.html")]
	public class SetSharedMaterial: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public ObjectVariable m_SharedMaterial;

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
			m_Rigidbody2D.sharedMaterial = (PhysicsMaterial2D)m_SharedMaterial.Value;
			return TaskStatus.Success;
		}
	}
}
