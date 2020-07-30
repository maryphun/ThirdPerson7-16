using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category ("UnityEngine/Camera")]
	[Tooltip ("Returns a ray going from camera through a screen point.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Camera.ScreenPointToRay.html")]
	public class ScreenPointToRay: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("")]
		public Vector3Variable position;
		[Shared]
		[NotRequired]
		public Vector3Variable m_Origin;
		[Shared]
		[NotRequired]
		public Vector3Variable m_Direction;

		private GameObject m_PrevGameObject;
		private Camera m_Camera;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Camera = m_gameObject.Value.GetComponent<Camera> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Camera == null) {
				Debug.LogWarning ("Missing Component of type Camera!");
				return TaskStatus.Failure;
			}
			Ray ray = m_Camera.ScreenPointToRay (position);
			this.m_Origin.Value = ray.origin;
			this.m_Direction.Value = ray.direction;

			return TaskStatus.Success;
		}
	}
}
