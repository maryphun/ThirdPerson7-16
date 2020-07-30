using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("Should kinematickinematic and kinematicstatic collisions be allowed?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D-useFullKinematicContacts.html")]
	public class SetUseFullKinematicContacts: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public BoolVariable m_UseFullKinematicContacts;

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
			m_Rigidbody2D.useFullKinematicContacts = m_UseFullKinematicContacts.Value;
			return TaskStatus.Success;
		}
	}
}
