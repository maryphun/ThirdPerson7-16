using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityText
{
	[Category ("UnityEngine/UI/Text")]
	[Tooltip ("Should the text be allowed to auto resized.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Text-resizeTextForBestFit.html")]
	public class SetResizeTextForBestFit: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public BoolVariable m_ResizeTextForBestFit;

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
			m_Text.resizeTextForBestFit = m_ResizeTextForBestFit.Value;
			return TaskStatus.Success;
		}
	}
}
