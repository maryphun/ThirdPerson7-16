using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Gets the value of the virtual axis identified by axisName with no smoothing filtering applied.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html")]
	public class GetAxisRaw: Action
	{
		public StringVariable axisName;
		[Shared]
		public FloatVariable m_AxisRaw;

		public override TaskStatus OnUpdate ()
		{
			m_AxisRaw.Value = Input.GetAxisRaw (axisName.Value);
			return TaskStatus.Success;
		}
	}
}
