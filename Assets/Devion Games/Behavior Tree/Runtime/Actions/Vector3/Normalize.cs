using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Makes this vector have a magnitude of 1.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Normalize.html")]
	public class Normalize: Action
	{
		public Vector3Variable value;
		[Shared]
		public Vector3Variable m_Normalize;

		public override TaskStatus OnUpdate ()
		{
			m_Normalize.Value = Vector3.Normalize (value);
			return TaskStatus.Success;
		}
	}
}
