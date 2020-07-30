using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	public class LockSelectionField : ToolbarItem
	{
		public override void OnGUI ()
		{
			bool isLocked = EditorPrefs.GetBool ("LockSelection");
			if (GUILayout.Button (new GUIContent ("Lock", "Lock Selection"), isLocked ? Styles.toolbarActiveButton : EditorStyles.toolbarButton, GUILayout.Width (50f))) {
				if (BehaviorSelection.activeObject != null) {
					EditorPrefs.SetBool ("LockSelection", !isLocked);
				} else {
					EditorUtility.DisplayDialog ("Unable to Lock Selection", "No GameObject or Template selected.", "OK");
				}
			}
		}
	}
}