using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Sets x,y,z component of the vector.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.html")]
	public class SetXYZ: Action
	{
		[Shared]
		public Vector3Variable vector;
		[NotRequired]
		public FloatVariable m_X;
		[NotRequired]
		public FloatVariable m_Y;
		[NotRequired]
		public FloatVariable m_Z;

		public override TaskStatus OnUpdate ()
		{
			if (!m_X.isNone) {
				vector.Value = new Vector3 (m_X.Value, vector.Value.y, vector.Value.z);
			}
			if (!m_Y.isNone) {
				vector.Value = new Vector3 (vector.Value.x, m_Y.Value, vector.Value.z);
			}
			if (!m_Z.isNone) {
				vector.Value = new Vector3 (vector.Value.x, vector.Value.y, m_Z.Value);
			}
			return TaskStatus.Success;
		}
	}
}
