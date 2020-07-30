using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAudioSource
{
	[Category("UnityEngine/AudioSource")]
	[Tooltip("Changes the time at which a sound that has already been scheduled to play will start.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/AudioSource.SetScheduledStartTime.html")]
	public class SetScheduledStartTime: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip("Time in seconds.")]
		public FloatVariable time;

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
			m_AudioSource.SetScheduledStartTime(time);
			return TaskStatus.Success;
		}
	}
}
