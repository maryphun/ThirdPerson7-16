using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	[InitializeOnLoad]
	public class HierarchyIcons
	{
		static HierarchyIcons ()
		{
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
		}

		static void HierarchyWindowItemCallback (int pID, Rect pRect)
		{
			GameObject go = EditorUtility.InstanceIDToObject (pID) as GameObject;
			if (go != null && go.GetComponent<Behavior> () != null) {
				Rect rect = new Rect (pRect.x + pRect.width - 5, pRect.y - 3f, 22, 22);
				if (Preferences.GetBool (Preference.ShowIconInHierarchy)) {
					if (GUI.Button (rect, Styles.logo, "Label")) {
						if (BehaviorTreeWindow.current == null) {
							BehaviorTreeWindow.ShowWindow ();
						}
						BehaviorSelection.activeObject = go.GetComponent<Behavior> ().gameObject;
						BehaviorSelection.activeBehavior = go.GetComponent<IBehavior> ();
						BehaviorTreeWindow.current.m_Graph.CenterGraphView ();
						BehaviorTreeWindow.current.Repaint ();
					}
				}
				IBehavior[] behaviors = go.GetComponents<IBehavior> ();
				bool hasErrors = false;
				for (int i = 0; i < behaviors.Length; i++) {
					IBehavior behavior = behaviors [i];
					//ErrorChecker.CheckForErrors (behavior);
					if (!hasErrors && ErrorChecker.HasErrors (behavior)) {
						hasErrors = true;
						Rect rect1 = new Rect (pRect.x + pRect.width  - rect.width, pRect.y - 6f, 29f, 29f);
						if (Preferences.GetBool (Preference.ShowErrorInHierarchy)) {
							if (GUI.Button (rect1, Styles.errorIcon, "label")) {
								ErrorWindow.ShowWindow ();
							}
						}
					}
				}
			}
		}
	}
}