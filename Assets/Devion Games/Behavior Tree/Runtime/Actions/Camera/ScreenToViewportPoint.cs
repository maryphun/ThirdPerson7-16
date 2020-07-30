using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category("UnityEngine/Camera")]
	[Tooltip("Transforms position from screen space into viewport space.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Camera.ScreenToViewportPoint.html")]
	public class ScreenToViewportPoint: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("")]
		public Vector3Variable position;
		[Shared]
		public Vector3Variable m_ScreenToViewportPoint;

		private GameObject m_PrevGameObject;
		private Camera m_Camera;

		public override void OnStart ()
		{
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_Camera = m_gameObject.Value.GetComponent<Camera>();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if(m_Camera == null)
			{
				Debug.LogWarning("Missing Component of type Camera!");
				return TaskStatus.Failure;
			}
			m_ScreenToViewportPoint.Value = m_Camera.ScreenToViewportPoint(position);
			return TaskStatus.Success;
		}
	}
}
