using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category ("UnityEngine/Vector2")]
	[Tooltip ("Gets x,y component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector2.html")]
	public class GetXY: Action
	{
		public Vector2Variable vector;
		[Shared]
		[NotRequired]
		public FloatVariable m_X;
		[Shared]
		[NotRequired]
		public FloatVariable m_Y;

		public override TaskStatus OnUpdate ()
		{
			m_X.Value = vector.Value.x;
			m_Y.Value = vector.Value.y;
			return TaskStatus.Success;
		}
	}
}
