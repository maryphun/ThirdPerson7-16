using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAudioSource
{
	[Category ("UnityEngine/AudioSource")]
	[Tooltip ("Sets a user-defined parameter of a custom spatializer effect that is attached to an AudioSource.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/AudioSource.SetSpatializerFloat.html")]
	public class SetSpatializerFloat: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Zero-based index of user-defined parameter to be set.")]
		public IntVariable index;
		[Tooltip ("New value of the user-defined parameter.")]
		public FloatVariable value;

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
			return m_AudioSource.SetSpatializerFloat (index, value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
