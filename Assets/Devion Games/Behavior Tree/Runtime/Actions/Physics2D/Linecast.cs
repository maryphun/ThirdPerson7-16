using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("Casts a line against colliders in the scene. A linecast is an imaginary line between two points in world space. Any object making contact with the beam can be detected and reported. This differs from the similar raycast in that raycasting specifies the line using an origin and direction.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics2D.Linecast.html")]
	public class Linecast : Action
	{
		[Tooltip ("Start point.")]
		public Vector2Variable m_StartPosition;
		[Tooltip ("End point.")]
		public Vector2Variable m_EndPosition;
		[Tooltip ("A Layer mask that is used to selectively ignore colliders when casting a ray.")]
		public LayerMask m_LayerMask = Physics2D.DefaultRaycastLayers;
		[NotRequired]
		[Tooltip ("Only include objects with a Z coordinate (depth) greater than or equal to this value. None or -1 for -infinity")]
		public FloatVariable m_MinDepth = -1;
		[NotRequired]
		[Tooltip ("Only include objects with a Z coordinate (depth) less than or equal to this value. None or -1 for infinity")]
		public FloatVariable m_MaxDepth = -1;

		public override TaskStatus OnUpdate ()
		{

			return Physics2D.Linecast (m_StartPosition.Value, m_EndPosition.Value, m_LayerMask, (m_MinDepth.isNone || m_MinDepth.Value == -1 ? -Mathf.Infinity : m_MinDepth.Value), (m_MaxDepth.isNone || m_MaxDepth.Value == -1 ? Mathf.Infinity : m_MaxDepth.Value)) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}