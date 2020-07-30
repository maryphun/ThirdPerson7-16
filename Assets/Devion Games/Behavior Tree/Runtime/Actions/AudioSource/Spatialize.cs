using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityAudioSource
{
	[Category ("UnityEngine/AudioSource")]
	[Tooltip ("Success if spatialization enabled.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/AudioSource-spatialize.html")]
	public class Spatialize: Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		private GameObject m_PrevGameObject;
		private AudioSource m_AudioSource;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_AudioSource = m_gameObject.Value.GetComponent<AudioSource> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_AudioSource == null) {
				Debug.LogWarning ("Missing Component of type AudioSource!");
				return TaskStatus.Failure;
			}
			return m_AudioSource.spatialize ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
