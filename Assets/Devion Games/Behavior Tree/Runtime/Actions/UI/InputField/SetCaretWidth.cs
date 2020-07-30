using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityInputField
{
	[Category ("UnityEngine/UI/InputField")]
	[Tooltip ("The width of the caret in pixels.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/InputField-caretWidth.html")]
	public class SetCaretWidth: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public IntVariable m_CaretWidth;

		private GameObject m_PrevGameObject;
		private InputField m_InputField;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_InputField = m_gameObject.Value.GetComponent<InputField> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_InputField == null) {
				Debug.LogWarning ("Missing Component of type InputField!");
				return TaskStatus.Failure;
			}
			m_InputField.caretWidth = m_CaretWidth.Value;
			return TaskStatus.Success;
		}
	}
}
