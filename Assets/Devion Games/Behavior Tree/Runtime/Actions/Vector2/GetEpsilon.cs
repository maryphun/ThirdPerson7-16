using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2-kEpsilon.html")]
	public class GetEpsilon: Action{
		[Shared]
		public FloatVariable m_Epsilon;

		public override TaskStatus OnUpdate (){
			 m_Epsilon.Value = Vector2.kEpsilon;
			return TaskStatus.Success;
		}
	}
}
