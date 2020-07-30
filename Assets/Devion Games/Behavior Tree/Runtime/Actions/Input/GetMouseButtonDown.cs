using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success during the frame the user pressed the given mouse button.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html")]
	public class GetMouseButtonDown: Conditional
	{
		public IntVariable button;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetMouseButtonDown (button) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
