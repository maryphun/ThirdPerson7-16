using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Gets x,y,z component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.html")]
	public class GetXYZ: Action
	{
		[Shared]
		public Vector3Variable vector;
		[Shared]
		[NotRequired]
		public FloatVariable m_X;
		[Shared]
		[NotRequired]
		public FloatVariable m_Y;
		[Shared]
		[NotRequired]
		public FloatVariable m_Z;

		public override TaskStatus OnUpdate ()
		{
			m_X.Value = vector.Value.x;
			m_Y.Value = vector.Value.y;
			m_Z.Value = vector.Value.z;
			return TaskStatus.Success;
		}
	}
}
