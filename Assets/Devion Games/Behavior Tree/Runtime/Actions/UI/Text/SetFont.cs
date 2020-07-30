using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityText
{
	[Category ("UnityEngine/UI/Text")]
	[Tooltip ("The Font used by the text.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Text-font.html")]
	public class SetFont: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public ObjectVariable m_Font;

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
			m_Text.font = (Font)m_Font.Value;
			return TaskStatus.Success;
		}
	}
}
