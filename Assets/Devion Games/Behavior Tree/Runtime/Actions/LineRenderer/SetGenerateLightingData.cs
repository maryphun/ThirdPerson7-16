using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("Configures a line to generate Normals and Tangents. With this data, Scene lighting can affect the line via Normal Maps and the Unity Standard Shader, or your own custom-built Shaders.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer-generateLightingData.html")]
	public class SetGenerateLightingData: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public BoolVariable m_GenerateLightingData;

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
			m_LineRenderer.generateLightingData =  m_GenerateLightingData;
			return TaskStatus.Success;
		}
	}
}
