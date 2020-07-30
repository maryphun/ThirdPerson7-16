using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTransform
{
	[Category ("UnityEngine/Transform")]
	[Tooltip ("Applies a rotation of eulerAngles.z degrees around the z axis, eulerAngles.x degrees around the x axis, and eulerAngles.y degrees around the y axis (in that order).")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Transform.Rotate.html")]
	public class Rotate: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Rotation to apply.")]
		public Vector3Variable eulerAngles;

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
			m_Transform.Rotate (eulerAngles);
			return TaskStatus.Success;
		}
	}
}
