using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("Casts a ray against colliders in the scene.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics2D.RaycastAll.html")]
	public class RaycastAll: Action
	{
		[Tooltip ("The point in 2D space where the ray originates.")]
		public Vector2Variable m_Origin;
		[Tooltip ("Vector representing the direction of the ray.")]
		public Vector2Variable m_Direction;
		[Tooltip ("Maximum distance over which to cast the ray.")]
		public FloatVariable m_MaxDistance = -1;
		[Tooltip ("Filter to detect Colliders only on certain layers.")]
		public LayerMask m_LayerMask = Physics2D.DefaultRaycastLayers;
		[Tooltip ("Only include objects with a Z coordinate (depth) greater than or equal to this value.")]
		public FloatVariable m_MinDepth = -1;
		[Tooltip ("Only include objects with a Z coordinate (depth) greater than or equal to this value.")]
		public FloatVariable m_MaxDepth = -1;

		[Tooltip ("Store the hit game objects.")]
		public ArrayVariable m_Store;


		public override TaskStatus OnUpdate ()
		{ 
			RaycastHit2D[] hits = Physics2D.RaycastAll (m_Origin.Value, m_Direction.Value, (m_MaxDistance.isNone || m_MaxDistance.Value == -1f ? Mathf.Infinity : m_MaxDistance.Value), m_LayerMask, (m_MinDepth.isNone || m_MinDepth.Value == -1f ? -Mathf.Infinity : m_MinDepth.Value), (m_MaxDepth.isNone || m_MaxDepth.Value == -1f ? Mathf.Infinity : m_MaxDepth.Value));
			m_Store.Value = hits.Select (x => x.collider.gameObject).ToArray ();
			return TaskStatus.Success;
		}
	}
}
