using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTime
{
	[Category ("UnityEngine/Time")]
	[Tooltip ("The time at the beginning of this frame.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Time-time.html")]
	[Icon ("Wait")]
	public class GetTime : Action
	{
		[Shared]
		[Tooltip ("Time")]
		public FloatVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Time.time;
			return TaskStatus.Success;
		}
	}
}