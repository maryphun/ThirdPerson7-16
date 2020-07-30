using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayableDirector
{
	[Category ("UnityEngine/Timeline/PlayableDirector")]
	[Tooltip ("Controls how the time is incremented when it goes beyond the duration of the playable.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayableDirector-extrapolationMode.html")]
	public class SetExtrapolationMode: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public DirectorWrapMode m_ExtrapolationMode;

		private GameObject m_PrevGameObject;
		private PlayableDirector m_PlayableDirector;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_PlayableDirector = m_gameObject.Value.GetComponent<PlayableDirector> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_PlayableDirector == null) {
				Debug.LogWarning ("Missing Component of type PlayableDirector!");
				return TaskStatus.Failure;
			}
			m_PlayableDirector.extrapolationMode = m_ExtrapolationMode;
			return TaskStatus.Success;
		}
	}
}
