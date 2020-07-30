using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTime
{
	[Category ("UnityEngine/Time")]
	[Tooltip ("The interval in seconds at which physics and other fixed frame rate updates are performed.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Time-fixedDeltaTime.html")]
	[Icon ("Wait")]
	public class GetFixedDeltaTime : Action
	{
		[Shared]
		[Tooltip ("Result.")]
		public FloatVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Time.fixedDeltaTime;
			return TaskStatus.Success;
		}
	}
}