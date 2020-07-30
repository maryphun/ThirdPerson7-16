using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category("UnityEngine/Camera")]
	[Tooltip("Transforms position from world space into screen space.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Camera.WorldToScreenPoint.html")]
	public class WorldToScreenPoint: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("")]
		public Vector3Variable position;
		[Shared]
		public Vector3Variable m_WorldToScreenPoint;

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
			m_WorldToScreenPoint.Value = m_Camera.WorldToScreenPoint(position);
			return TaskStatus.Success;
		}
	}
}
