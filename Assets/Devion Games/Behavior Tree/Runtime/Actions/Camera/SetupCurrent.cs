using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category("UnityEngine/Camera")]
	[Tooltip("")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Camera.SetupCurrent.html")]
	public class SetupCurrent: Action
	{
		[Tooltip("")]
		public Camera cur;

		public override TaskStatus OnUpdate ()
		{
			Camera.SetupCurrent(cur);
			return TaskStatus.Success;
		}
	}
}
