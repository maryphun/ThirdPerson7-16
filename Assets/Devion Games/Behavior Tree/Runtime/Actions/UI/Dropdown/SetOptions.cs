using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityDropdown
{
	[Category ("UnityEngine/UI/Dropdown")]
	[Tooltip ("The list of possible options. A text string and an image can be specified for each option.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Dropdown-options.html")]
	public class SetOptions: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public List<StringVariable> m_Options;

		private GameObject m_PrevGameObject;
		private Dropdown m_Dropdown;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Dropdown = m_gameObject.Value.GetComponent<Dropdown> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Dropdown == null) {
				Debug.LogWarning ("Missing Component of type Dropdown!");
				return TaskStatus.Failure;
			}
			List<Dropdown.OptionData> options = new List<Dropdown.OptionData> ();

			for (int i = 0; i < m_Options.Count; i++) {
				Dropdown.OptionData data = new Dropdown.OptionData (m_Options [i].Value);
				options.Add (data);
			}
			m_Dropdown.options = options;
			return TaskStatus.Success;
		}
	}
}
