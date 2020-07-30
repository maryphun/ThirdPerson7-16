using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Sets y component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.html")]
	public class SetY: Action
	{
		[Shared]
		public Vector3Variable vector;
		public FloatVariable m_Y;


		public override TaskStatus OnUpdate ()
		{
			vector.Value = new Vector3 (vector.Value.x ,m_Y.Value, vector.Value.z);
			return TaskStatus.Success;
		}
	}
}
