using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success the first frame the user releases the virtual button identified by buttonName.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetButtonUp.html")]
	public class GetButtonUp: Conditional
	{
		public StringVariable buttonName;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetButtonUp (buttonName.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
