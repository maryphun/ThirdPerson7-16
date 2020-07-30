using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLineRenderer
{
	[Category("UnityEngine/LineRenderer")]
	[Tooltip("Creates a snapshot of LineRenderer and stores it in mesh.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/LineRenderer.BakeMesh.html")]
	public class BakeMesh: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("A static mesh that will receive the snapshot of the line.")]
		public Mesh mesh;
		[Tooltip("The camera used for determining which way camera-space lines will face.")]
		public BoolVariable useTransform;

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
			m_LineRenderer.BakeMesh(mesh, useTransform);
			return TaskStatus.Success;
		}
	}
}
