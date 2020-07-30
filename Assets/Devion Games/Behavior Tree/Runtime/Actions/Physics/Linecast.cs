using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("Returns success if there is any collider intersecting the line between start and end.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics.Linecast.html")]
	public class Linecast : Action
	{
		[Tooltip ("Start point.")]
		public Vector3Variable m_StartPosition;
		[Tooltip ("End point.")]
		public Vector3Variable m_EndPosition;
		[Tooltip ("A Layer mask that is used to selectively ignore colliders when casting a ray.")]
		public LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
		[Tooltip ("Specifies whether this query should hit Triggers")]
		public QueryTriggerInteraction queryTriggerInteraction;

		public override TaskStatus OnUpdate ()
		{
			return Physics.Linecast (m_StartPosition.Value, m_EndPosition.Value, m_LayerMask, queryTriggerInteraction) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}