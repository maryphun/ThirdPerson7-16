using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Reflects a vector off the plane defined by a normal.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Reflect.html")]
	public class Reflect: Action
	{
		public Vector3Variable inDirection;
		public Vector3Variable inNormal;
		[Shared]
		public Vector3Variable m_Reflect;

		public override TaskStatus OnUpdate ()
		{
			m_Reflect.Value = Vector3.Reflect (inDirection, inNormal);
			return TaskStatus.Success;
		}
	}
}
