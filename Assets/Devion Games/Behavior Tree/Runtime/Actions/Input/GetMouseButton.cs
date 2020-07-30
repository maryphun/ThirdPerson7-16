using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success if the given mouse button is held down.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetMouseButton.html")]
	public class GetMouseButton: Conditional
	{
		public IntVariable button;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetMouseButton (button) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
