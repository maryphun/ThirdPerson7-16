using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category("UnityEngine/Vector3")]
	[Tooltip("")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector3-kEpsilon.html")]
	public class GetEpsilon: Action{
		[Shared]
		public FloatVariable m_Epsilon;

		public override TaskStatus OnUpdate (){
			 m_Epsilon.Value = Vector3.kEpsilon;
			return TaskStatus.Success;
		}
	}
}
