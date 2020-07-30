using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Is any key or mouse button currently held down?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input-anyKey.html")]
	public class AnyKey: Conditional
	{
		public override TaskStatus OnUpdate ()
		{
			return Input.anyKey ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
