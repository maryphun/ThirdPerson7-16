using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Lenght of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Magnitude.html")]
	public class Magnitude: Action
	{
		public Vector3Variable a;
		[Shared]
		public FloatVariable m_Magnitude;

		public override TaskStatus OnUpdate ()
		{
			m_Magnitude.Value = Vector3.Magnitude (a);
			return TaskStatus.Success;
		}
	}
}
