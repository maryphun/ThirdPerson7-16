using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTransform
{
	[Category ("UnityEngine/Transform")]
	[Tooltip ("The number of transforms in the transform's hierarchy data structure.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Transform-hierarchyCount.html")]
	public class GetHierarchyCount: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public IntVariable m_HierarchyCount;

		private GameObject m_PrevGameObject;
		private Transform m_Transform;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Transform = m_gameObject.Value.GetComponent<Transform> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Transform == null) {
				Debug.LogWarning ("Missing Component of type Transform!");
				return TaskStatus.Failure;
			}
			m_HierarchyCount.Value = m_Transform.hierarchyCount;
			return TaskStatus.Success;
		}
	}
}
