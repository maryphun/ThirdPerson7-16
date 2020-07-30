using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success during the frame the user pressed down the virtual button identified by buttonName.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetButtonDown.html")]
	public class GetButtonDown: Conditional
	{
		public StringVariable buttonName;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetButtonDown (buttonName.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
