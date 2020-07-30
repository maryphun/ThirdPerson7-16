using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("Set the position of a vertex in the line.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer.SetPosition.html")]
	public class SetPosition: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("Which position to set.")]
		public IntVariable index;
		[Tooltip("The new position.")]
		public Vector3Variable position;

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
			m_LineRenderer.SetPosition(index, position);
			return TaskStatus.Success;
		}
	}
}
