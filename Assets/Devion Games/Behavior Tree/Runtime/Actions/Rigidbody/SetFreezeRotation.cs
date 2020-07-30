using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("Controls whether physics will change the rotation of the object.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody-freezeRotation.html")]
	public class SetFreezeRotation: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public BoolVariable m_FreezeRotation;

		private GameObject m_PrevGameObject;
		private Rigidbody m_Rigidbody;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Rigidbody = m_gameObject.Value.GetComponent<Rigidbody> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Rigidbody == null) {
				Debug.LogWarning ("Missing Component of type Rigidbody!");
				return TaskStatus.Failure;
			}
			m_Rigidbody.freezeRotation = m_FreezeRotation.Value;
			return TaskStatus.Success;
		}
	}
}
