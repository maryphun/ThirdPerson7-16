using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("Casts a sphere along a ray and returns detailed information on what was hit.This is useful when a Raycast does not give enough precision, because you want to find out if an object of a specific size, such as a character, will be able to move somewhere without colliding with anything on the way. Think of the sphere cast like a thick raycast. In this case the ray is specified by a start vector and a direction.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics.SphereCast.html")]
	public class SphereCast : Action
	{
		[Tooltip ("The center of the sphere at the start of the sweep.")]
		public Vector3Variable m_Origin;
		[Tooltip ("The direction into which to sweep the sphere.")]
		public Vector3Variable m_Direction;
		[Tooltip ("The radius of the sphere.")]
		public FloatVariable m_Radius = 1f;
		[NotRequired]
		[Tooltip ("The max length of the cast. -1f or None for infinity.")]
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
			if (Physics.SphereCast (m_Origin.Value, m_Radius.Value, m_Direction.Value, out hit, (m_MaxDistance.isNone || m_MaxDistance.Value == -1f ? Mathf.Infinity : m_MaxDistance.Value), m_LayerMask, queryTriggerInteraction)) {
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