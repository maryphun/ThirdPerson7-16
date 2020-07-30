using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.BehaviorTrees.Actions.UnityNavMeshAgent
{
	[Category ("UnityEngine/NavMeshAgent")]
	[Tooltip ("Gets the next OffMeshLinkData on the current path.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/NavMeshAgent-nextOffMeshLinkData.html")]
	public class GetNextOffMeshLinkData: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Header ("Result")]
		[Shared]
		[NotRequired]
		[Tooltip ("Is link active")]
		public BoolVariable activated;
		[Shared]
		[NotRequired]
		[Tooltip ("Link start world position")]
		public Vector3Variable startPosition;
		[Shared]
		[NotRequired]
		[Tooltip ("Link end world position")]
		public Vector3Variable endPosition;
		[Shared]
		[NotRequired]
		[Tooltip ("Is link valid")]
		public BoolVariable valid;

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
			OffMeshLinkData nextOffMeshLinkData = m_NavMeshAgent.nextOffMeshLinkData;
			activated.Value = nextOffMeshLinkData.activated;
			startPosition.Value = nextOffMeshLinkData.startPos;
			endPosition.Value = nextOffMeshLinkData.endPos;
			valid.Value = nextOffMeshLinkData.valid;
			return TaskStatus.Success;
		}
	}
}
