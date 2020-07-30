using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTime
{
	[Category ("UnityEngine/Time")]
	[Tooltip ("The scale at which the time is passing. This can be used for slow motion effects.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Time-timeScale.html")]
	[Icon ("Wait")]
	public class GetTimeScale : Action
	{
		[Shared]
		[Tooltip ("Time scale")]
		public FloatVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Time.timeScale;
			return TaskStatus.Success;
		}
	}
}