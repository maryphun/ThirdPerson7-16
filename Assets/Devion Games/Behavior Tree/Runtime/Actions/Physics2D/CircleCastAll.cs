using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("Casts a circle against colliders in the scene.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics2D.CircleCastAll.html")]
	public class CircleCastAll : Action
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

		[Tooltip ("Store the hit game objects.")]
		public ArrayVariable m_Store;


		public override TaskStatus OnUpdate ()
		{ 
			RaycastHit2D[] hits = Physics2D.CircleCastAll (m_Origin.Value, m_Radius.Value, m_Direction.Value, (m_MaxDistance.isNone || m_MaxDistance.Value == -1f ? Mathf.Infinity : m_MaxDistance.Value), m_LayerMask, (m_MinDepth.isNone || m_MinDepth.Value == -1f ? -Mathf.Infinity : m_MinDepth.Value), (m_MaxDepth.isNone || m_MaxDepth.Value == -1f ? Mathf.Infinity : m_MaxDepth.Value));
			m_Store.Value = hits.Select (x => x.collider.gameObject).ToArray ();
			return TaskStatus.Success;
		}
	}
}
