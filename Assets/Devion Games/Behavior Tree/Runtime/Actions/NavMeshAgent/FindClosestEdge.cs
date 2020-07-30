using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.BehaviorTrees.Actions.UnityNavMeshAgent
{
	[Category ("UnityEngine/NavMeshAgent")]
	[Tooltip ("Locate the closest NavMesh edge. Returns success if nearest edge was found.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/NavMeshAgent.FindClosestEdge.html")]
	public class FindClosestEdge: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Header ("Result")]
		[Shared]
		[NotRequired]
		[Tooltip ("Distance to the point of hit.")]
		public FloatVariable distance;
		[Shared]
		[NotRequired]
		[Tooltip ("Mask specifying NavMesh area at point of hit.")]
		public IntVariable mask;
		[Shared]
		[NotRequired]
		[Tooltip ("Normal at the point of hit.")]
		public Vector3Variable normal;
		[Shared]
		[NotRequired]
		[Tooltip ("Position of hit.")]
		public Vector3Variable m_position;

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
			NavMeshHit hit;
			if (m_NavMeshAgent.FindClosestEdge (out hit)) {
				distance.Value = hit.distance;
				mask.Value = hit.mask;
				normal.Value = hit.normal;
				m_position.Value = hit.position;
				return TaskStatus.Success;
			}
			
			return TaskStatus.Failure;
		}
	}
}
