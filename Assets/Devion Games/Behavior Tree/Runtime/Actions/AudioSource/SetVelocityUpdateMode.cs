using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAudioSource
{
	[Category ("UnityEngine/AudioSource")]
	[Tooltip ("Whether the Audio Source should be updated in the fixed or dynamic update.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/AudioSource-velocityUpdateMode.html")]
	public class SetVelocityUpdateMode: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public AudioVelocityUpdateMode m_VelocityUpdateMode;

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
			m_AudioSource.velocityUpdateMode = m_VelocityUpdateMode;
			return TaskStatus.Success;
		}
	}
}
