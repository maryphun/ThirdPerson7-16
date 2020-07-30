using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees
{
	[CustomEditor (typeof(Behavior))]
	public class BehaviorInspector : Editor
	{

        public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
			EditorGUI.BeginChangeCheck ();
			SerializedProperty script = serializedObject.FindProperty ("m_Script");
			SerializedProperty behavior = serializedObject.FindProperty ("m_BehaviorTree");
			SerializedProperty template = serializedObject.FindProperty ("m_Template");
			bool enabled = GUI.enabled;
			GUI.enabled = false;
			EditorGUILayout.PropertyField (script);
			GUI.enabled = enabled;


			SerializedObject templateObject = null;
			if (template != null && template.objectReferenceValue != null) {
				templateObject = new SerializedObject (template.objectReferenceValue);
				behavior = templateObject.FindProperty ("m_BehaviorTree");
				templateObject.Update ();
			}

			EditorGUILayout.PropertyField (behavior.FindPropertyRelative ("m_Name"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_TickEvent"));


			if (!(BehaviorSelection.activeBehavior is BehaviorTemplate)) {

				Object cur = template.objectReferenceValue;
				EditorGUILayout.PropertyField (template);
				if (cur != template.objectReferenceValue && BehaviorTreeWindow.current != null) {
					BehaviorTreeWindow.current.m_Graph.m_Selection.Clear ();
					BehaviorTreeWindow.current.m_Graph.CenterGraphView ();
					BehaviorTreeWindow.current.Repaint ();
				}
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("startWhenEnabled"));
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("restartWhenComplete"));
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("checkConfiguration"));
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("showSceneIcon"));
			} 

			EditorGUILayout.PropertyField (behavior.FindPropertyRelative ("description"));

			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
				if (templateObject != null) {
					templateObject.ApplyModifiedProperties ();
				}
			}
		}

		[DrawGizmo(GizmoType.Selected | GizmoType.NonSelected, typeof(Behavior))]
		private static void OnSceneGUI(Behavior target, GizmoType type)
		{
			if (target.showSceneIcon)
			{
				Transform transform = (target as Behavior).transform;
				Texture2D texture = Resources.Load<Texture2D>("BehaviorLogo");
				float handleSize = HandleUtility.GetHandleSize(transform.position);

				Handles.BeginGUI();
				Vector3 pos = transform.position;
				Vector2 pos2D = HandleUtility.WorldToGUIPoint(pos);
				float size = Mathf.Clamp(texture.width / handleSize, 0, texture.width);
				GUI.Label(new Rect(pos2D.x - size * 0.5f, pos2D.y - size * 0.5f, size, size), texture);
				Handles.EndGUI();
			}
		}
	}
}