using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success the first frame the user hits any key or mouse button.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input-anyKeyDown.html")]
	public class AnyKeyDown: Conditional
	{
		
		public override TaskStatus OnUpdate ()
		{
			return Input.anyKeyDown ? TaskStatus.Success : TaskStatus.Failure;
		}

	}
}
