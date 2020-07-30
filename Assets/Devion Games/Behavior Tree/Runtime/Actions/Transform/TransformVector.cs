using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTransform
{
	[Category ("UnityEngine/Transform")]
	[Tooltip ("Transforms vector from local space to world space.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Transform.TransformVector.html")]
	public class TransformVector: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("")]
		public Vector3Variable vector;
		[Shared]
		public Vector3Variable m_TransformVector;

		private GameObject m_PrevGameObject;
		private Transform m_Transform;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Transform = m_gameObject.Value.GetComponent<Transform> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Transform == null) {
				Debug.LogWarning ("Missing Component of type Transform!");
				return TaskStatus.Failure;
			}
			m_TransformVector.Value = m_Transform.TransformVector (vector);
			return TaskStatus.Success;
		}
	}
}
