using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.BehaviorTrees.Conditionals.UnityNavMeshAgent
{
	[Category ("UnityEngine/NavMeshAgent")]
	[Tooltip ("Returns success if the agent moves across OffMeshLinks automatically.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/NavMeshAgent-autoTraverseOffMeshLink.html")]
	public class AutoTraverseOffMeshLink: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		private GameObject m_PrevGameObject;
		private NavMeshAgent m_NavMeshAgent;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_NavMeshAgent = m_gameObject.Value.GetComponent<NavMeshAgent> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_NavMeshAgent == null) {
				Debug.LogWarning ("Missing Component of type NavMeshAgent!");
				return TaskStatus.Failure;
			}
			return m_NavMeshAgent.autoTraverseOffMeshLink ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
