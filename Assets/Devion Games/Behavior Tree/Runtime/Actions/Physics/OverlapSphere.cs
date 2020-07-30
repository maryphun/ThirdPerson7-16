using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("Gets an array with all game objects touching or inside the sphere.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html")]
	public class OverlapSphere : Action
	{
		[Tooltip ("Center of the sphere.")]
		public Vector3Variable m_Center;
		[Tooltip ("The radius of the sphere.")]
		public FloatVariable m_Radius = 1f;
		[Tooltip ("A Layer mask that is used to selectively ignore colliders when casting a ray.")]
		public LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
		[Tooltip ("Specifies whether this query should hit Triggers")]
		public QueryTriggerInteraction queryTriggerInteraction;

		[Shared]
		[Tooltip ("Store the game objects touching or inside the sphere.")]
		public ArrayVariable m_Store;

		public override TaskStatus OnUpdate ()
		{

			Collider[] colliders = Physics.OverlapSphere (m_Center.Value, m_Radius.Value, m_LayerMask, queryTriggerInteraction);
			m_Store.Value = colliders.Select (x => x.gameObject).ToArray ();
			return TaskStatus.Success;
		}
	}
}