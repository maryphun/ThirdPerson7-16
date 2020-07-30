using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	public class StepField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (Styles.step, EditorStyles.toolbarButton)) {
				EditorApplication.Step ();
			}
		}
	}
}