using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityAudioSource
{
	[Category("UnityEngine/AudioSource")]
	[Tooltip("When set doesn't route the signal from an AudioSource into the global reverb associated with reverb zones.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/AudioSource-bypassReverbZones.html")]
	public class BypassReverbZones: Conditional{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

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
			return m_AudioSource.bypassReverbZones ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
