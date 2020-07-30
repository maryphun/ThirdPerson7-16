using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAudioSource
{
	[Category("UnityEngine/AudioSource")]
	[Tooltip("Set the custom curve for the given AudioSourceCurveType.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/AudioSource.SetCustomCurve.html")]
	public class SetCustomCurve: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("The curve type that should be set.")]
		public AudioSourceCurveType type;
		[Tooltip("The curve that should be applied to the given curve type.")]
		public AnimationCurve curve;

		private GameObject m_PrevGameObject;
		private AudioSource m_AudioSource;

		public override void OnStart ()
		{
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_AudioSource = m_gameObject.Value.GetComponent<AudioSource>();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if(m_AudioSource == null)
			{
				Debug.LogWarning("Missing Component of type AudioSource!");
				return TaskStatus.Failure;
			}
			m_AudioSource.SetCustomCurve(type, curve);
			return TaskStatus.Success;
		}
	}
}
