using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTransform
{
	[Category ("UnityEngine/Transform")]
	[Tooltip ("Rotates the transform so the forward vector points at target's current position.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Transform.LookAt.html")]
	public class LookAt: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[NotRequired]
		[Tooltip ("Object to point towards.")]
		public TransformVariable target;
		[NotRequired]
		[Tooltip("Position to point towards.")]
		public Vector3Variable m_TargetPosition;
		[Tooltip("Forces the game object to remain vertical. Useful for characters.")]
		public BoolVariable m_KeepVertical;

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

			Vector3 lookAt = (target.Value == null || target.isNone) ? m_TargetPosition.Value : target.Value.position;
			if (!m_KeepVertical.isNone) {
				lookAt.y = m_Transform.position.y;
			}
			m_Transform.LookAt(lookAt);
		
			return TaskStatus.Success;
		}
	}
}
