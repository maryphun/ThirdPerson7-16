using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success while the virtual button identified by buttonName is held down.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetButton.html")]
	public class GetButton: Conditional
	{
		public StringVariable buttonName;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetButton (buttonName.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
