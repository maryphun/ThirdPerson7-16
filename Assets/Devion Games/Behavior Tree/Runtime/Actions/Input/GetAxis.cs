using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Gets the value of the virtual axis identified by axisName.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetAxis.html")]
	public class GetAxis: Action
	{
		public StringVariable axisName;
		[Shared]
		public FloatVariable m_Axis;

		public override TaskStatus OnUpdate ()
		{
			m_Axis.Value = Input.GetAxis (axisName.Value);
			return TaskStatus.Success;
		}
	}
}
