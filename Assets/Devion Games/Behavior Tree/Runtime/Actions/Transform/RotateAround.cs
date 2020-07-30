using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTransform
{
	[Category ("UnityEngine/Transform")]
	[Tooltip ("Rotates the transform about axis passing through point in world coordinates by angle degrees.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Transform.RotateAround.html")]
	public class RotateAround: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("")]
		public Vector3Variable point;
		[Tooltip ("")]
		public Vector3Variable axis;
		[Tooltip ("")]
		public FloatVariable angle;

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
			m_Transform.RotateAround (point, axis, angle);
			return TaskStatus.Success;
		}
	}
}
