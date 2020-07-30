using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("Get the positions of all vertices in the line.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer.GetPositions.html")]
	public class GetPositions: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("The array of positions to retrieve. The array passed should be of at least positionCount in size.")]
		public Vector3[] positions;
		[Shared]
		public IntVariable m_GetPositions;

		private GameObject m_PrevGameObject;
		private LineRenderer m_LineRenderer;

		public override void OnStart ()
		{
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_LineRenderer = m_gameObject.Value.GetComponent<LineRenderer>();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if(m_LineRenderer == null)
			{
				Debug.LogWarning("Missing Component of type LineRenderer!");
				return TaskStatus.Failure;
			}
			m_GetPositions.Value = m_LineRenderer.GetPositions(positions);
			return TaskStatus.Success;
		}
	}
}
