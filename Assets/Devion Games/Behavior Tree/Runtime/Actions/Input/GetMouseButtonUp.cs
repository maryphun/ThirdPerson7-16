using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success during the frame the user releases the given mouse button.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonUp.html")]
	public class GetMouseButtonUp: Conditional
	{
		public IntVariable button;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetMouseButtonUp (button) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
