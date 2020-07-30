using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("Casts a ray through the scene and stores all hit game objects. Note that order is not guaranteed.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics.RaycastAll.html")]
	public class RaycastAll : Action
	{
		[Tooltip ("The starting point of the ray in world coordinates.")]
		public Vector3Variable m_Origin;
		[Tooltip ("The direction of the ray.")]
		public Vector3Variable m_Direction;
		[NotRequired]
		[Tooltip ("The max distance the rayhit is allowed to be from the start of the ray. -1 or None for infinity.")]
		public FloatVariable m_MaxDistance = -1f;
		[Tooltip ("A Layer mask that is used to selectively ignore colliders when casting a ray.")]
		public LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
		[Tooltip ("Specifies whether this query should hit Triggers")]
		public QueryTriggerInteraction queryTriggerInteraction;

		[Tooltip ("Store the hit game objects.")]
		public ArrayVariable m_Store;


		public override TaskStatus OnUpdate ()
		{
			RaycastHit[] hits = Physics.RaycastAll (m_Origin.Value, m_Direction.Value, (m_MaxDistance.isNone || m_MaxDistance.Value == -1f ? Mathf.Infinity : m_MaxDistance.Value), m_LayerMask, queryTriggerInteraction);
			m_Store.Value = hits.Select (x => x.collider.gameObject).ToArray ();
			return TaskStatus.Success;
		}
	}
}