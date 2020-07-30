using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityDropdown
{
	[Category("UnityEngine/UI/Dropdown")]
	[Tooltip("The Value is the index number of the current selection in the Dropdown. 0 is the first option in the Dropdown, 1 is the second, and so on.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Dropdown-value.html")]
	public class GetValue: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public IntVariable m_Value;

		private GameObject m_PrevGameObject;
		private Dropdown m_Dropdown;

		public override void OnStart (){
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_Dropdown = m_gameObject.Value.GetComponent<Dropdown>();
			}
		}

		public override TaskStatus OnUpdate (){
			if(m_Dropdown == null){
				Debug.LogWarning("Missing Component of type Dropdown!");
				return TaskStatus.Failure;
			}
			 m_Value.Value = m_Dropdown.value;
			return TaskStatus.Success;
		}
	}
}
