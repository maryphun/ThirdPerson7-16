using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTransform
{
	[Category ("UnityEngine/Transform")]
	[Tooltip ("Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Transform.InverseTransformDirection.html")]
	public class InverseTransformDirection: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("")]
		public Vector3Variable direction;
		[Shared]
		public Vector3Variable m_InverseTransformDirection;

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
			m_InverseTransformDirection.Value = m_Transform.InverseTransformDirection (direction);
			return TaskStatus.Success;
		}
	}
}
