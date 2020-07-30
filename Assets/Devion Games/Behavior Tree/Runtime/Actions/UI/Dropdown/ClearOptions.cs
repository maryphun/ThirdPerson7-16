using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityDropdown
{
	[Category("UnityEngine/UI/Dropdown")]
	[Tooltip("Clear the list of options in the Dropdown.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Dropdown.ClearOptions.html")]
	public class ClearOptions: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		private GameObject m_PrevGameObject;
		private Dropdown m_Dropdown;

		public override void OnStart ()
		{
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_Dropdown = m_gameObject.Value.GetComponent<Dropdown>();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if(m_Dropdown == null)
			{
				Debug.LogWarning("Missing Component of type Dropdown!");
				return TaskStatus.Failure;
			}
			m_Dropdown.ClearOptions();
			return TaskStatus.Success;
		}
	}
}
