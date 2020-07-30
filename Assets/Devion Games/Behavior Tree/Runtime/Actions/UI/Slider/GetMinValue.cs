using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnitySlider
{
	[Category("UnityEngine/UI/Slider")]
	[Tooltip("The minimum allowed value of the slider.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Slider-minValue.html")]
	public class GetMinValue: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_MinValue;

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
			 m_MinValue.Value = m_Slider.minValue;
			return TaskStatus.Success;
		}
	}
}
