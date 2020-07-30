using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category ("UnityEngine/Vector2")]
	[Tooltip ("Makes this vector have a magnitude of 1.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector2.Normalize.html")]
	public class Normalize: Action
	{
		public Vector2Variable value;
		[Shared]
		public Vector2Variable m_Normalize;

		public override TaskStatus OnUpdate ()
		{
			m_Normalize.Value = value.Value.normalized;
			return TaskStatus.Success;
		}
	}
}
