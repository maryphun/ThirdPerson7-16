using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayableDirector
{
	[Category ("UnityEngine/Timeline/PlayableDirector")]
	[Tooltip ("The time at which the Playable should start when first played.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayableDirector-initialTime.html")]
	public class GetInitialTime: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_InitialTime;

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
			m_InitialTime.Value = (float)m_PlayableDirector.initialTime;
			return TaskStatus.Success;
		}
	}
}
