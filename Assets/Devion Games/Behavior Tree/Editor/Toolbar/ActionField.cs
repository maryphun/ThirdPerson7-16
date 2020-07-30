using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevionGames.BehaviorTrees
{
	public class ActionField : ToolbarItem
	{
		Action m_Action;

		public ActionField (Action action)
		{
			this.m_Action = action;
		}

		public override void OnGUI ()
		{
			if (this.m_Action != null) {
				this.m_Action.Invoke ();
			}
		}
	}
}