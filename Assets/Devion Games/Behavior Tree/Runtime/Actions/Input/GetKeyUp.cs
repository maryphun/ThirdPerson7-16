using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success during the frame the user releases the key identified by name.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetKeyUp.html")]
	public class GetKeyUp: Conditional
	{
		public StringVariable m_KeyName;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetKeyUp (m_KeyName.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
