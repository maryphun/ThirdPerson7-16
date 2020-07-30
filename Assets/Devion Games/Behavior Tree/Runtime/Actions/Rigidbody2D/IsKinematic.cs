using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityRigidbody2D
{
	[Category ("UnityEngine/Rigidbody2D")]
	[Tooltip ("Should this rigidbody be taken out of physics control?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody2D-isKinematic.html")]
	public class IsKinematic: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public BoolVariable m_IsKinematic;

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
			return m_Rigidbody2D.isKinematic ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
