using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("Like Physics.SphereCast, but this action will get all hit game objects the sphere sweep intersects.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics.SphereCastAll.html")]
	public class SphereCastAll : Action
	{
		[Tooltip ("The center of the sphere at the start of the sweep.")]
		public Vector3Variable m_Origin;
		[Tooltip ("The direction into which to sweep the sphere.")]
		public Vector3Variable m_Direction;
		[Tooltip ("The radius of the sphere.")]
		public FloatVariable m_Radius = 1f;
		[NotRequired]
		[Tooltip ("The max length of the cast. -1 or None for infinity.")]
		public FloatVariable m_MaxDistance = -1f;
		[Tooltip ("A Layer mask that is used to selectively ignore colliders when casting a ray.")]
		public LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
		[Tooltip ("Specifies whether this query should hit Triggers")]
		public QueryTriggerInteraction queryTriggerInteraction;

		[Tooltip ("Store the hit game objects.")]
		public ArrayVariable m_Store;


		public override TaskStatus OnUpdate ()
		{
			RaycastHit[] hits = Physics.SphereCastAll (m_Origin.Value, m_Radius.Value, m_Direction.Value, (m_MaxDistance.isNone || m_MaxDistance.Value == -1f ? Mathf.Infinity : m_MaxDistance.Value), m_LayerMask, queryTriggerInteraction);
			m_Store.Value = hits.Select (x => x.collider.gameObject).ToArray ();
			return TaskStatus.Success;
		}
	}
}