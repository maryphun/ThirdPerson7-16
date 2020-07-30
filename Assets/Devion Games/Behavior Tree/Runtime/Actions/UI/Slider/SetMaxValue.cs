using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnitySlider
{
	[Category("UnityEngine/UI/Slider")]
	[Tooltip("The maximum allowed value of the slider.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Slider-maxValue.html")]
	public class SetMaxValue: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public FloatVariable m_MaxValue;

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
			m_Slider.maxValue =  m_MaxValue.Value;
			return TaskStatus.Success;
		}
	}
}
