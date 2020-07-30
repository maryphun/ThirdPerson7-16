using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityCamera
{
	[Category("UnityEngine/Camera")]
	[Tooltip("Should the camera clear the stencil buffer after the deferred light pass?")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Camera-clearStencilAfterLightingPass.html")]
	public class ClearStencilAfterLightingPass: Conditional{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

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
			return m_Camera.clearStencilAfterLightingPass ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
