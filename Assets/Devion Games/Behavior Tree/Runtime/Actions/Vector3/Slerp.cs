using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Spherically interpolates between two vectors.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Slerp.html")]
	public class Slerp: Action
	{
		public Vector3Variable a;
		public Vector3Variable b;
		public FloatVariable t;
		[Shared]
		public Vector3Variable m_Slerp;

		public override TaskStatus OnUpdate ()
		{
			m_Slerp.Value = Vector3.Slerp (a, b, t);
			return TaskStatus.Success;
		}
	}
}
