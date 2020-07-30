using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTime
{
	[Category ("UnityEngine/Time")]
	[Tooltip ("The time this frame has started. This is the time in seconds since the last level has been loaded.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Time-timeSinceLevelLoad.html")]
	[Icon ("Wait")]
	public class GetTimeSinceLevelLoad : Action
	{
		[Shared]
		[Tooltip ("Result.")]
		public FloatVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Time.timeSinceLevelLoad;
			return TaskStatus.Success;
		}
	}
}