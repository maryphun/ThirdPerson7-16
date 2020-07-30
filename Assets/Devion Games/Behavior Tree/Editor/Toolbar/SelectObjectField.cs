using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class SelectObjectField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (new GUIContent (BehaviorSelection.activeObject != null ? BehaviorSelection.activeObject.name : "(None Selected)", "Select GameObject or Template"), EditorStyles.toolbarDropDown, GUILayout.Width (100f))) {
				GenericMenu menu = new GenericMenu ();

				IBehavior[] behaviors = Resources.FindObjectsOfTypeAll<UnityEngine.Object> ().Where (x => typeof(IBehavior).IsAssignableFrom (x.GetType ())).Select (y => y as IBehavior).ToArray ();
				GUIContent[] names = behaviors.Select (x => new GUIContent (string.Concat (x.GetObject ().name, (x.GetObject () is ScriptableObject) ? " (Template)" : ""))).ToArray ();
				for (int i = 0; i < names.Length; i++) {
					int index = i;

					menu.AddItem (names [index], BehaviorSelection.activeObject != null ? behaviors [index].GetObject ().name == BehaviorSelection.activeObject.name : false, delegate() {
						Selection.activeObject = behaviors [index].GetObject ();
					});
				}

				menu.ShowAsContext ();
				EditorGUIUtility.ExitGUI ();
			}
		}
	}
}