using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("Set the color at the start of the line.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer-startColor.html")]
	public class GetStartColor: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public ColorVariable m_StartColor;

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
			 m_StartColor.Value = m_LineRenderer.startColor;
			return TaskStatus.Success;
		}
	}
}
