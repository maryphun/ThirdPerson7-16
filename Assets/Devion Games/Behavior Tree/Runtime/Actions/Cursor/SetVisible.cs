using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCursor
{
	[Category("UnityEngine/Cursor")]
	[Tooltip("Determines whether the hardware pointer is visible or not.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Cursor-visible.html")]
	public class SetVisible: Action{
		public BoolVariable m_Visible;

		public override TaskStatus OnUpdate (){
			Cursor.visible =  m_Visible;
			return TaskStatus.Success;
		}
	}
}
