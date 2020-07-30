using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTime
{
	[Category ("UnityEngine/Time")]
	[Tooltip ("The time in seconds it took to complete the last frame")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Time-deltaTime.html")]
	[Icon ("Wait")]
	public class GetDeltaTime : Action
	{
		[Shared]
		[Tooltip ("Delta time")]
		public FloatVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Time.deltaTime;
			return TaskStatus.Success;
		}
	}
}