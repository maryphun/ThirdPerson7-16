using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityTransform
{
	[Category ("UnityEngine/Transform")]
	[Tooltip ("Has the transform changed since the last time the flag was set to 'false'?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Transform-hasChanged.html")]
	public class HasChanged: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

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
			return m_Transform.hasChanged ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
