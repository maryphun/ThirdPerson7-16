using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCursor
{
	[Category("UnityEngine/Cursor")]
	[Tooltip("Determines whether the hardware pointer is locked to the center of the view, constrained to the window, or not constrained at all.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Cursor-lockState.html")]
	public class SetLockState: Action{
		public CursorLockMode m_LockState;

		public override TaskStatus OnUpdate (){
			Cursor.lockState =  m_LockState;
			return TaskStatus.Success;
		}
	}
}
