using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("If enabled, the lines are defined in world space.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer-useWorldSpace.html")]
	public class UseWorldSpace: Conditional{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		private GameObject m_PrevGameObject;
		private LineRenderer m_LineRenderer;

		public override void OnStart (){
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_LineRenderer = m_gameObject.Value.GetComponent<LineRenderer>();
			}
		}

		public override TaskStatus OnUpdate (){
			if(m_LineRenderer == null){
				Debug.LogWarning("Missing Component of type LineRenderer!");
				return TaskStatus.Failure;
			}
			return m_LineRenderer.useWorldSpace ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
