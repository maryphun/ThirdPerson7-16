using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Multiplies two vectors component-wise.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Scale.html")]
	public class Scale: Action
	{
		public Vector3Variable a;
		public Vector3Variable b;
		[Shared]
		public Vector3Variable m_Scale;

		public override TaskStatus OnUpdate ()
		{
			m_Scale.Value = Vector3.Scale (a, b);
			return TaskStatus.Success;
		}
	}
}
