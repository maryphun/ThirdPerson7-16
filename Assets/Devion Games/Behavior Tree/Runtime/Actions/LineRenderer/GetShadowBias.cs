using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("Apply a shadow bias to prevent self-shadowing artifacts. The specified value is the proportion of the line width at each segment.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer-shadowBias.html")]
	public class GetShadowBias: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_ShadowBias;

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
			 m_ShadowBias.Value = m_LineRenderer.shadowBias;
			return TaskStatus.Success;
		}
	}
}
