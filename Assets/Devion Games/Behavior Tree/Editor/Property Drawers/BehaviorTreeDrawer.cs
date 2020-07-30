using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	[CustomPropertyDrawer (typeof(BehaviorTree))]
	public class BehaviorTreeDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{

			EditorGUI.BeginChangeCheck ();
			Rect rect = position;
			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField (rect, property.FindPropertyRelative ("m_Name"));
			rect.y += EditorGUIUtility.singleLineHeight;

			SerializedProperty template = property.serializedObject.FindProperty ("m_Template");
			if (template != null) {
				Object cur = template.objectReferenceValue;
				EditorGUI.PropertyField (rect, property.serializedObject.FindProperty ("m_Template"));
				rect.y += EditorGUIUtility.singleLineHeight;
				if (template.objectReferenceValue != cur) {
					if (BehaviorTreeWindow.current != null) {
						BehaviorTreeWindow.current.m_Graph.m_Selection.Clear ();
						BehaviorTreeWindow.current.m_Graph.CenterGraphView ();
						BehaviorTreeWindow.current.Repaint ();
					}

				}
			}
			rect.height = EditorGUI.GetPropertyHeight (property.FindPropertyRelative ("description"));
			EditorGUI.PropertyField (rect, property.FindPropertyRelative ("description"));

			/*rect.y += EditorGUIUtility.singleLineHeight;
			rect.height = EditorGUI.GetPropertyHeight (property.FindPropertyRelative ("serializationData"));
			EditorGUI.PropertyField (rect, property.FindPropertyRelative ("serializationData"));*/

			if (EditorGUI.EndChangeCheck () && BehaviorTreeWindow.current != null) {
				BehaviorTreeWindow.current.m_Graph.m_Selection.Clear ();
				ErrorChecker.CheckForErrors ();
				BehaviorTreeWindow.current.Repaint ();
			}

		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight (property, label) - (property.serializedObject.FindProperty ("m_Template") != null ? 0f : EditorGUIUtility.singleLineHeight);
		}
	}
}