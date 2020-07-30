using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("The current mouse position in pixel coordinates.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Input-mousePosition.html")]
	public class GetMousePosition: Action
	{
		[Shared]
		public Vector3Variable m_MousePosition;

		public override TaskStatus OnUpdate ()
		{
			m_MousePosition.Value = Input.mousePosition;
			return TaskStatus.Success;
		}
	}
}
