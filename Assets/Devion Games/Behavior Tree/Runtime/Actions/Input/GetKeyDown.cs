using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success during the frame the user starts pressing down the key identified by name.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html")]
	public class GetKeyDown: Conditional
	{
		public StringVariable m_KeyName;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetKeyDown (m_KeyName.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
