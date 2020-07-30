using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Sets z component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.html")]
	public class SetZ: Action
	{
		[Shared]
		public Vector3Variable vector;
		public FloatVariable m_Z;


		public override TaskStatus OnUpdate ()
		{
			vector.Value = new Vector3 (vector.Value.x , vector.Value.y, m_Z.Value);
			return TaskStatus.Success;
		}
	}
}
