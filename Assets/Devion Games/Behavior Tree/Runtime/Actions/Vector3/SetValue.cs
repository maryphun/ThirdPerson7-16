using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Sets x,y,z component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.html")]
	public class SetValue: Action
	{
		[Shared]
		public Vector3Variable vector;
		public Vector3Variable m_value;

		public override TaskStatus OnUpdate ()
		{
			
			vector.Value = m_value.Value;
		
			return TaskStatus.Success;
		}
	}
}
