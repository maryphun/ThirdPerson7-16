using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityText
{
	[Category ("UnityEngine/UI/Text")]
	[Tooltip ("The size that the Font should render at.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Text-fontSize.html")]
	public class GetFontSize: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public IntVariable m_FontSize;

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
			m_FontSize.Value = m_Text.fontSize;
			return TaskStatus.Success;
		}
	}
}
