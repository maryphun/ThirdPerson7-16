using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("Checks whether any of the collider(s) attached to this rigidbody are touching any colliders on the specified layerMask or not.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D.IsTouchingLayers.html")]
	public class IsTouchingLayers: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Any colliders on any of these layers count as touching.")]
		public LayerMask layerMask;
	
		private GameObject m_PrevGameObject;
		private Rigidbody2D m_Rigidbody2D;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Rigidbody2D = m_gameObject.Value.GetComponent<Rigidbody2D> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Rigidbody2D == null) {
				Debug.LogWarning ("Missing Component of type Rigidbody2D!");
				return TaskStatus.Failure;
			}
			return m_Rigidbody2D.IsTouchingLayers (layerMask) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
