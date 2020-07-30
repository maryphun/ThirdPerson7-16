using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityDropdown
{
	[Category ("UnityEngine/UI/Dropdown")]
	[Tooltip ("Add multiple options to the options of the Dropdown based on a list of OptionData objects.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Dropdown.AddOptions.html")]
	public class AddOptions: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The list of OptionData to add.")]
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
			List<string> options = new List<string> ();
			for (int i = 0; i < m_Options.Count; i++) {
				options.Add (m_Options [i].Value);
			}

			m_Dropdown.AddOptions (options);
			return TaskStatus.Success;
		}
	}
}
