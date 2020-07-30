using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category ("UnityEngine/Vector2")]
	[Tooltip ("Sets x,y component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector2-x.html")]
	public class SetXY: Action
	{
		[Shared]
		public Vector2Variable vector;
		[NotRequired]
		public FloatVariable m_X;
		[NotRequired]
		public FloatVariable m_Y;


		public override TaskStatus OnUpdate ()
		{
			if (!m_X.isNone) {
				vector.Value = new Vector2 (m_X.Value, vector.Value.y);
			}
			if (!m_Y.isNone) {
				vector.Value = new Vector2 (vector.Value.x, m_Y.Value);
			}
			return TaskStatus.Success;
		}
	}
}
