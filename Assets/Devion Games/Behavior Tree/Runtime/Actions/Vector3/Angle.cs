using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Gets the angle in degrees between from and to.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Angle.html")]
	public class Angle: Action
	{
		[Tooltip ("The angle extends round from this vector.")]
		public Vector3Variable from;
		[Tooltip ("The angle extends round to this vector.")]
		public Vector3Variable to;
		[Shared]
		public FloatVariable m_Angle;

		public override TaskStatus OnUpdate ()
		{
			m_Angle.Value = Vector3.Angle (from, to);
			return TaskStatus.Success;
		}
	}
}
