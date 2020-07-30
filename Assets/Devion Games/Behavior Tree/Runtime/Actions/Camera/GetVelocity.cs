using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category("UnityEngine/Camera")]
	[Tooltip("Get the world-space speed of the camera (Read Only).")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Camera-velocity.html")]
	public class GetVelocity: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public Vector3Variable m_Velocity;

		private GameObject m_PrevGameObject;
		private Camera m_Camera;

		public override void OnStart (){
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_Camera = m_gameObject.Value.GetComponent<Camera>();
			}
		}

		public override TaskStatus OnUpdate (){
			if(m_Camera == null){
				Debug.LogWarning("Missing Component of type Camera!");
				return TaskStatus.Failure;
			}
			 m_Velocity.Value = m_Camera.velocity;
			return TaskStatus.Success;
		}
	}
}
