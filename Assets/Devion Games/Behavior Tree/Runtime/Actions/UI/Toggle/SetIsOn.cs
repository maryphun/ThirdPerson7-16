using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityToggle
{
	[Category("UnityEngine/UI/Toggle")]
	[Tooltip("Return or set whether the Toggle is on or not.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Toggle-isOn.html")]
	public class SetIsOn: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public BoolVariable m_IsOn;

		private GameObject m_PrevGameObject;
		private Toggle m_Toggle;

		public override void OnStart (){
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_Toggle = m_gameObject.Value.GetComponent<Toggle>();
			}
		}

		public override TaskStatus OnUpdate (){
			if(m_Toggle == null){
				Debug.LogWarning("Missing Component of type Toggle!");
				return TaskStatus.Failure;
			}
			m_Toggle.isOn =  m_IsOn.Value;
			return TaskStatus.Success;
		}
	}
}
