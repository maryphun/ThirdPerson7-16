using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("Adds a force to the rigidbody2D relative to its coordinate system.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D.AddRelativeForce.html")]
	public class AddRelativeForce: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Components of the force in the X and Y axes.")]
		public Vector2Variable relativeForce;
		[Tooltip ("The method used to apply the specified force.")]
		public ForceMode2D mode;

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
			m_Rigidbody2D.AddRelativeForce (relativeForce, mode);
			return TaskStatus.Success;
		}
	}
}
