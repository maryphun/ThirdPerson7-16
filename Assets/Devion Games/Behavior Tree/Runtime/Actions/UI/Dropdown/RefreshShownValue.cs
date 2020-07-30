using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityDropdown
{
	[Category ("UnityEngine/UI/Dropdown")]
	[Tooltip ("Refreshes the text and image (if available) of the currently selected option. If you have modified the list of options, you should call this method afterwards to ensure that the visual state of the dropdown corresponds to the updated options.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Dropdown.RefreshShownValue.html")]
	public class RefreshShownValue: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

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
			m_Dropdown.RefreshShownValue ();
			return TaskStatus.Success;
		}
	}
}
