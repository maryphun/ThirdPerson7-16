using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("Get a local space point given the point point in rigidBody global space.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D.GetPoint.html")]
	public class GetPoint: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The global space point to transform into local space.")]
		public Vector2Variable point;
		[Shared]
		public Vector2Variable m_StorePoint;

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
			m_StorePoint.Value = m_Rigidbody2D.GetPoint (point);
			return TaskStatus.Success;
		}
	}
}
