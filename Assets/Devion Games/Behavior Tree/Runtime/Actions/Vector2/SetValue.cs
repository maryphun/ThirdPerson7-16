using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category ("UnityEngine/Vector2")]
	[Tooltip ("Sets x,y component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector2-x.html")]
	public class SetValue: Action
	{
		[Shared]
		public Vector2Variable vector;
		public Vector2Variable m_value;


		public override TaskStatus OnUpdate ()
		{

			vector.Value = m_value.Value;

			return TaskStatus.Success;
		}
	}
}
