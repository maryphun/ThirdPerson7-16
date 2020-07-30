using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAudioSource
{
	[Category ("UnityEngine/AudioSource")]
	[Tooltip ("Plays an AudioClip at a given position in world space.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/AudioSource.PlayClipAtPoint.html")]
	public class PlayClipAtPoint: Action
	{
		[Tooltip ("Audio data to play.")]
		public AudioClip clip;
		[Tooltip ("Position in world space from which sound originates.")]
		public Vector3Variable m_position;

		public override TaskStatus OnUpdate ()
		{
			AudioSource.PlayClipAtPoint (clip, m_position);
			return TaskStatus.Success;
		}
	}
}
