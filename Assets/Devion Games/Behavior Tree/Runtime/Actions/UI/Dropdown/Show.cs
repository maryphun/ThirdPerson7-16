using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityDropdown
{
	[Category("UnityEngine/UI/Dropdown")]
	[Tooltip("Show the dropdown list.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Dropdown.Show.html")]
	public class Show: Action
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
			m_Dropdown.Show();
			return TaskStatus.Success;
		}
	}
}
