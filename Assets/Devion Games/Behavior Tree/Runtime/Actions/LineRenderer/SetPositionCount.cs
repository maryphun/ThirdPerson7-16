using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("Set/get the number of vertices.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer-positionCount.html")]
	public class SetPositionCount: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public IntVariable m_PositionCount;

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
			m_LineRenderer.positionCount =  m_PositionCount;
			return TaskStatus.Success;
		}
	}
}
