using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTime
{
	[Category ("UnityEngine/Time")]
	[Tooltip ("The scale at which the time is passing. This can be used for slow motion effects.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Time-timeScale.html")]
	[Icon ("Wait")]
	public class SetTimeScale : Action
	{
		[Shared]
		[Tooltip ("Time scale")]
		public FloatVariable m_Scale;

		public override TaskStatus OnUpdate ()
		{
			Time.timeScale = m_Scale.Value;
			return TaskStatus.Success;
		}
	}
}