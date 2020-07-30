using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class SelectBehaviorField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (new GUIContent (BehaviorSelection.activeBehavior != null ? BehaviorSelection.activeBehaviorTree.name : "(None Selected)", "Select Behavior"), EditorStyles.toolbarDropDown, GUILayout.Width (100f))) {
				GenericMenu menu = new GenericMenu ();
				if (BehaviorSelection.activeGameObject != null) {

					IBehavior[] behaviors = BehaviorSelection.activeGameObject.GetComponents<IBehavior> ();
					GUIContent[] names = behaviors.Select (x => new GUIContent (x.GetBehaviorTree ().name)).ToArray ();

					for (int i = 0; i < names.Length; i++) {
						int index = i;
						menu.AddItem (names [index], behaviors [index] == BehaviorSelection.activeBehavior, delegate() {
							BehaviorSelection.activeBehavior = behaviors [index];
						});
					}
				}
				menu.ShowAsContext ();
				EditorGUIUtility.ExitGUI ();
			}
		}
	}
}