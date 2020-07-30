using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Sets x component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.html")]
	public class SetX: Action
	{
		[Shared]
		public Vector3Variable vector;
		public FloatVariable m_X;


		public override TaskStatus OnUpdate ()
		{
			vector.Value = new Vector3 (m_X.Value, vector.Value.y, vector.Value.z);
			return TaskStatus.Success;
		}
	}
}
