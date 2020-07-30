using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Moves a point current towards target.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.MoveTowards.html")]
	public class MoveTowards: Action
	{
		[Tooltip("")]
		public Vector2Variable current;
		[Tooltip("")]
		public Vector2Variable target;
		[Tooltip("")]
		public FloatVariable maxDistanceDelta;
		[Shared]
		public Vector2Variable m_MoveTowards;

		public override TaskStatus OnUpdate ()
		{
			m_MoveTowards.Value = Vector2.MoveTowards(current, target, maxDistanceDelta);
			return TaskStatus.Success;
		}
	}
}
