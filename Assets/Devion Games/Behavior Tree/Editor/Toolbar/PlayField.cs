using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	public class PlayField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (EditorApplication.isPlaying ? Styles.playOn : Styles.playOff, EditorApplication.isPlaying ? Styles.toolbarActiveButton : EditorStyles.toolbarButton)) {
				EditorApplication.isPlaying = !EditorApplication.isPlaying;
			}
		}
	}
}