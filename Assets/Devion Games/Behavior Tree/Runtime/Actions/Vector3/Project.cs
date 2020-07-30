using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Projects a vector onto another vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Project.html")]
	public class Project: Action
	{
		public Vector3Variable vector;
		public Vector3Variable onNormal;
		[Shared]
		public Vector3Variable m_Project;

		public override TaskStatus OnUpdate ()
		{
			m_Project.Value = Vector3.Project (vector, onNormal);
			return TaskStatus.Success;
		}
	}
}
