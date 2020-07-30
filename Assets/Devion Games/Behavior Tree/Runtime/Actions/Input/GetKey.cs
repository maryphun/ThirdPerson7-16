using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Returns success while the user holds down the key identified by name. Think auto fire.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input.GetKey.html")]
	public class GetKey: Conditional
	{
		public StringVariable m_KeyName;

		public override TaskStatus OnUpdate ()
		{
			return Input.GetKey (m_KeyName.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
