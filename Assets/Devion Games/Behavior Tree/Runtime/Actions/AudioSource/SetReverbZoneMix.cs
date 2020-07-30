using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAudioSource
{
	[Category("UnityEngine/AudioSource")]
	[Tooltip("The amount by which the signal from the AudioSource will be mixed into the global reverb associated with the Reverb Zones.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/AudioSource-reverbZoneMix.html")]
	public class SetReverbZoneMix: Action{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public FloatVariable m_ReverbZoneMix;

		private GameObject m_PrevGameObject;
		private AudioSource m_AudioSource;

		public override void OnStart (){
			if(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){
				m_PrevGameObject=m_gameObject.Value;
				m_AudioSource = m_gameObject.Value.GetComponent<AudioSource>();
			}
		}

		public override TaskStatus OnUpdate (){
			if(m_AudioSource == null){
				Debug.LogWarning("Missing Component of type AudioSource!");
				return TaskStatus.Failure;
			}
			m_AudioSource.reverbZoneMix =  m_ReverbZoneMix.Value;
			return TaskStatus.Success;
		}
	}
}
