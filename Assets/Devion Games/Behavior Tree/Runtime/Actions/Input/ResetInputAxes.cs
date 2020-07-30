using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Resets all input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.ResetInputAxes.html")]
	public class ResetInputAxes: Action
	{

		public override TaskStatus OnUpdate ()
		{
			Input.ResetInputAxes ();
			return TaskStatus.Success;
		}
	}
}
