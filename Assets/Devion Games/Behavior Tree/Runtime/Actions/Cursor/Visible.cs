using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityCursor
{
	[Category("UnityEngine/Cursor")]
	[Tooltip("Determines whether the hardware pointer is visible or not.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Cursor-visible.html")]
	public class Visible: Conditional{

		public override TaskStatus OnUpdate (){
			return Cursor.visible ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
