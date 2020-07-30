using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Returns the distance between a and b.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Distance.html")]
	public class Distance: Action
	{
		public Vector3Variable a;
		public Vector3Variable b;
		public Vector3Variable scale = Vector3.one;
		[Shared]
		public FloatVariable m_Distance;

		public override TaskStatus OnUpdate ()
		{
			m_Distance.Value = Vector3.Distance (Vector3.Scale (a, scale.Value), Vector3.Scale (b, scale.Value));
			return TaskStatus.Success;
		}
	}
}
