using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class ErrorWindow : EditorWindow
	{
		private bool selectedBehavior;
		private Vector2 scroll;
		private int index = -1;

		[MenuItem ("Tools/Devion Games/Behavior Trees/Error Checker", false, 3)]
		public static void ShowWindow ()
		{
			ErrorWindow window = EditorWindow.GetWindow<ErrorWindow> ("Error Checker");
			Vector2 size = new Vector2 (250f, 200f);
			window.minSize = size;
			window.selectedBehavior = EditorPrefs.GetBool ("Selected Only", false);
			window.wantsMouseMove = true;
		}

		private void OnEnable ()
		{
			ErrorChecker.CheckForErrors ();

		}

		private void OnFocus ()
		{
			Repaint ();
		}

		private void OnGUI ()
		{
			List<TaskError> errors = ErrorChecker.GetErrors ().OrderBy (x => x.behavior.GetBehaviorTree ().name).ToList ();
			if (selectedBehavior) {
				if (BehaviorSelection.activeBehaviorTree != null) {
					errors = errors.FindAll (x => x.behavior == BehaviorSelection.activeBehavior).ToList ();
				} else {
					errors.Clear ();
				}
			}
			//Toolbar
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			if (GUILayout.Button ("Refresh", EditorStyles.toolbarButton)) {
				ErrorChecker.CheckForErrors ();
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Selected Only", (selectedBehavior ? Styles.toolbarActiveButton : EditorStyles.toolbarButton))) {
				selectedBehavior = !selectedBehavior;
				ErrorChecker.CheckForErrors ();
				EditorPrefs.SetBool ("Selected Only", selectedBehavior);

			}
			GUILayout.EndHorizontal ();

			scroll = EditorGUILayout.BeginScrollView (scroll);
			for (int i = 0; i < errors.Count; i++) {
				TaskError error = errors [i];
				GUIStyle style = Styles.elementBackground;
				if (i == index) {
					style = new GUIStyle ("MeTransitionSelectHead") {
						stretchHeight = false,

					};
					style.overflow = new RectOffset (-1, -2, -2, 2);
				}
				GUILayout.BeginVertical (style);
				GUILayout.Label (ObjectNames.NicifyVariableName (error.type.ToString ()));
				GUILayout.Label (error.behavior.GetObject ().name + " : " + error.behavior.GetBehaviorTree ().name + " : " + ObjectNames.NicifyVariableName (error.node.GetType ().Name));		
				GUILayout.EndVertical ();
				Rect elementRect = new Rect (0, i * 19f * 2f, Screen.width, 19f * 2f);
				Event ev = Event.current;
				switch (ev.rawType) {
				case EventType.MouseDown:
					if (elementRect.Contains (Event.current.mousePosition)) {
						if (Event.current.button == 0) {
							index = i;
							if (BehaviorTreeWindow.current == null) {
								BehaviorTreeWindow.ShowWindow ();
							}
							BehaviorSelection.activeObject = error.behavior.GetObject ();
							BehaviorSelection.activeBehavior = error.behavior;
							BehaviorTreeWindow.current.m_Graph.m_Selection.Clear ();
							BehaviorTreeWindow.current.m_Graph.m_Selection.Add (error.node);
							BehaviorTreeWindow.current.m_Graph.CenterGraphView ();
							BehaviorTreeWindow.current.Repaint ();
							if (BehaviorTreeWindow.current.graphInspectorWindow != null) {
								BehaviorTreeWindow.current.graphInspectorWindow.Repaint ();
							}
							Event.current.Use ();
						} 
					}
					break;
				}
			}
			EditorGUILayout.EndScrollView ();
		}

	}
}