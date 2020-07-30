using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	public class PauseField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (EditorApplication.isPaused ? Styles.pauseOn : Styles.pauseOff, EditorApplication.isPaused ? Styles.toolbarActiveButton : EditorStyles.toolbarButton)) {
				EditorApplication.isPaused = !EditorApplication.isPaused;
			}
		}
	}
}