using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.BehaviorTrees.Actions.UnityNavMeshAgent
{
	[Category ("UnityEngine/NavMeshAgent")]
	[Tooltip ("Sets the destination of the agent in world-space units.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/NavMeshAgent-destination.html")]
	public class SetDestination: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public Vector3Variable m_Destination;

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
			m_NavMeshAgent.destination = m_Destination.Value;
			return TaskStatus.Success;
		}
	}
}
