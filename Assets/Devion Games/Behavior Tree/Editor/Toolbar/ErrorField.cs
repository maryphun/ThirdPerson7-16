using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	public class ErrorField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (BehaviorSelection.activeBehavior != null && ErrorChecker.HasErrors (BehaviorSelection.activeBehavior)) {
				TaskError[] errors = ErrorChecker.GetErrors (BehaviorSelection.activeBehavior);
				if (GUILayout.Button (new GUIContent (string.Concat (errors.Length, " Error", (errors.Length <= 1 ? string.Empty : "s")), Styles.errorIcon), EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width (85f) })) {
					ErrorWindow.ShowWindow ();
				}
			}
		}

	}
}