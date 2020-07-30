using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayableDirector
{
	[Category ("UnityEngine/Timeline/PlayableDirector")]
	[Tooltip ("Controls how time is incremented when playing back.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayableDirector-timeUpdateMode.html")]
	public class SetTimeUpdateMode: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public DirectorUpdateMode m_TimeUpdateMode;

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
			m_PlayableDirector.timeUpdateMode = m_TimeUpdateMode;
			return TaskStatus.Success;
		}
	}
}
