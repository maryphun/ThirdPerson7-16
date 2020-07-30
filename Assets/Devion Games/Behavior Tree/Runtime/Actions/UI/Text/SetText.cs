using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityText
{
	[Category ("UnityEngine/UI/Text")]
	[Tooltip ("The string value this text will display.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Text-text.html")]
	public class SetText: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public StringVariable m_Text;

		private GameObject m_PrevGameObject;
		private Text m_TextObject;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_TextObject = m_gameObject.Value.GetComponent<Text> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_TextObject == null) {
				Debug.LogWarning ("Missing Component of type Text!");
				return TaskStatus.Failure;
			}
			m_TextObject.text = m_Text.Value;
			return TaskStatus.Success;
		}
	}
}
