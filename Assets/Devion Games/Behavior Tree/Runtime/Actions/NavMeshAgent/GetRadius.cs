using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.BehaviorTrees.Actions.UnityNavMeshAgent
{
	[Category ("UnityEngine/NavMeshAgent")]
	[Tooltip ("Gets the avoidance radius for the agent.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/NavMeshAgent-radius.html")]
	public class GetRadius: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_Radius;

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
			m_Radius.Value = m_NavMeshAgent.radius;
			return TaskStatus.Success;
		}
	}
}
