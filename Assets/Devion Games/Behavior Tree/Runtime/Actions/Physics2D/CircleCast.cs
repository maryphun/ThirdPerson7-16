using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("Casts a circle against colliders in the scene.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics2D.CircleCast.html")]
	public class CircleCast: Action
	{
		[Tooltip ("The point in 2D space where the shape originates.")]
		public Vector2Variable m_Origin;
		[Tooltip ("The radius of the shape.")]
		public FloatVariable m_Radius;
		[Tooltip ("Vector representing the direction of the shape.")]
		public Vector2Variable m_Direction;
		[Tooltip ("Maximum distance over which to cast the shape.")]
		public FloatVariable m_MaxDistance = -1;
		[Tooltip ("Filter to detect Colliders only on certain layers.")]
		public LayerMask m_LayerMask = Physics2D.DefaultRaycastLayers;
		[Tooltip ("Only include objects with a Z coordinate (depth) greater than or equal to this value.")]
		public FloatVariable m_MinDepth = -1;
		[Tooltip ("\tOnly include objects with a Z coordinate (depth) less than or equal to this value.")]
		public FloatVariable m_MaxDepth = -1;

		[Header ("Result")]
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit game object.")]
		public GameObjectVariable m_StoreObject;
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit point.")]
		public Vector2Variable m_StorePoint;
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit normal.")]
		public Vector2Variable m_StoreNormal;
		[Shared]
		[NotRequired]
		[Tooltip ("Store the hit distance.")]
		public FloatVariable m_StoreDistance;

		public override TaskStatus OnUpdate ()
		{ 
			RaycastHit2D hit = Physics2D.CircleCast (m_Origin.Value, m_Radius.Value, m_Direction.Value, (m_MaxDistance.isNone || m_MaxDistance.Value == -1f ? Mathf.Infinity : m_MaxDistance.Value), m_LayerMask, (m_MinDepth.isNone || m_MinDepth.Value == -1f ? -Mathf.Infinity : m_MinDepth.Value), (m_MaxDepth.isNone || m_MaxDepth.Value == -1f ? Mathf.Infinity : m_MaxDepth.Value));
			if (hit.collider != null) {
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
