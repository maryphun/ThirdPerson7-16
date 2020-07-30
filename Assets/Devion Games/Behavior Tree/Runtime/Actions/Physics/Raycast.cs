using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("Casts a ray against all colliders in the scene.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics.Raycast.html")]
	public class Raycast : Action
	{
		[Tooltip ("The starting point of the ray in world coordinates.")]
		public Vector3Variable m_Origin;
		[Tooltip ("The direction of the ray.")]
		public Vector3Variable m_Direction;
		[NotRequired]
		[Tooltip ("The max distance the ray should check for collisions. -1 or None for infinity.")]
		public FloatVariable m_MaxDistance = -1f;
		[Tooltip ("A Layer mask that is used to selectively ignore colliders when casting a ray.")]
		public LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
		[Tooltip ("Specifies whether this query should hit Triggers")]
		public QueryTriggerInteraction queryTriggerInteraction;
		[Header ("Result")]
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit game object.")]
		public GameObjectVariable m_StoreObject;
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit point.")]
		public Vector3Variable m_StorePoint;
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit normal.")]
		public Vector3Variable m_StoreNormal;
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit distance.")]
		public FloatVariable m_StoreDistance;

		public override TaskStatus OnUpdate ()
		{

			RaycastHit hit;
			if (Physics.Raycast (m_Origin.Value, m_Direction.Value, out hit, (m_MaxDistance.isNone || m_MaxDistance.Value == -1f ? Mathf.Infinity : m_MaxDistance.Value), m_LayerMask, queryTriggerInteraction)) {
				this.m_StoreObject.Value = hit.collider.gameObject;
				this.m_StorePoint.Value = hit.point;
				this.m_StoreDistance.Value = hit.distance;
				this.m_StoreNormal.Value = hit.normal;
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}
	}
}