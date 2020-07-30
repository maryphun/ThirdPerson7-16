using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category("UnityEngine/Camera")]
	[Tooltip("Camera's half-size when in orthographic mode.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Camera-orthographicSize.html")]
	public class GetOrthographicSize: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_OrthographicSize;

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
			 m_OrthographicSize.Value = m_Camera.orthographicSize;
			return TaskStatus.Success;
		}
	}
}
