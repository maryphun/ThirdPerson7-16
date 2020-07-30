using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Projects a vector onto a plane defined by a normal orthogonal to the plane.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.ProjectOnPlane.html")]
	public class ProjectOnPlane: Action
	{
		public Vector3Variable vector;
		public Vector3Variable planeNormal;
		[Shared]
		public Vector3Variable m_ProjectOnPlane;

		public override TaskStatus OnUpdate ()
		{
			m_ProjectOnPlane.Value = Vector3.ProjectOnPlane (vector, planeNormal);
			return TaskStatus.Success;
		}
	}
}
