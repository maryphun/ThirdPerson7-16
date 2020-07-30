using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnitySlider
{
	[Category("UnityEngine/UI/Slider")]
	[Tooltip("The current value of the slider normalized into a value between 0 and 1.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Slider-normalizedValue.html")]
	public class SetNormalizedValue: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public FloatVariable m_NormalizedValue;

		private GameObject m_PrevGameObject;
		private Slider m_Slider;

		public override void OnStart (){
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_Slider = m_gameObject.Value.GetComponent<Slider>();
			}
		}

		public override TaskStatus OnUpdate (){
			if(m_Slider == null){
				Debug.LogWarning("Missing Component of type Slider!");
				return TaskStatus.Failure;
			}
			m_Slider.normalizedValue =  m_NormalizedValue.Value;
			return TaskStatus.Success;
		}
	}
}
