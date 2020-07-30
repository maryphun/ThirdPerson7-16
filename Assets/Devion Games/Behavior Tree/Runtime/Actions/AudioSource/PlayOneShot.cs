using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAudioSource
{
	[Category("UnityEngine/AudioSource")]
	[Tooltip("Plays an AudioClip, and scales the AudioSource volume by volumeScale.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html")]
	public class PlayOneShot: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("The clip being played.")]
		public AudioClip clip;
		[Tooltip("The scale of the volume (0-1).")]
		public FloatVariable volumeScale;

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
			m_AudioSource.PlayOneShot(clip, volumeScale);
			return TaskStatus.Success;
		}
	}
}
