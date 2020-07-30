using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("Get a global space vector given the vector relativeVector in rigidBody local space.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D.GetRelativeVector.html")]
	public class GetRelativeVector: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The local space vector to transform into a global space vector.")]
		public Vector2Variable relativeVector;
		[Shared]
		public Vector2Variable m_StoreRelativeVector;

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
			m_StoreRelativeVector.Value = m_Rigidbody2D.GetRelativeVector (relativeVector);
			return TaskStatus.Success;
		}
	}
}
