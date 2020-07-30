using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayableDirector
{
	[Category ("UnityEngine/Timeline/PlayableDirector")]
	[Tooltip ("Instatiates a Playable using the provided PlayableAsset and starts playback.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayableDirector.Play.html")]
	public class Play: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		[Tooltip ("An asset to instantiate a playable from.")]
		[NotRequired]
		public ObjectVariable asset;

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
			if (asset.Value != null) {
				m_PlayableDirector.Play ((PlayableAsset)asset.Value);
			} else {
				m_PlayableDirector.Play ();
			}
			return TaskStatus.Success;
		}
	}
}
