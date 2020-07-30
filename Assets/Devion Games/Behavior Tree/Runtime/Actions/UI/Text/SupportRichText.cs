using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Conditionals.UnityText
{
	[Category ("UnityEngine/UI/Text")]
	[Tooltip ("Whether this Text will support rich text.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Text-supportRichText.html")]
	public class SupportRichText: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		private GameObject m_PrevGameObject;
		private Text m_Text;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Text = m_gameObject.Value.GetComponent<Text> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Text == null) {
				Debug.LogWarning ("Missing Component of type Text!");
				return TaskStatus.Failure;
			}
			return m_Text.supportRichText ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
