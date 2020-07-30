using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	[CustomObjectDrawer (typeof(Variable))]
	public class VariableDrawer : ObjectDrawer
	{
		public override void OnGUI (GUIContent label)
		{

			Variable variable = value as Variable;
			GUILayout.BeginHorizontal ();
			bool sharedOnly = AttributeUtility.HasAttribute (fieldInfo, typeof(SharedAttribute)) || fieldInfo.FieldType == typeof(ArrayVariable);
			if (sharedOnly) {
				variable.isShared = true;
			}
			label = new GUIContent (label.text, fieldInfo.GetTooltip ());
		

			if (variable.isShared) {
				DrawSharedVariable (variable, label);
			} else {
				DrawVariableValue (variable, label);
			}

			if (!sharedOnly) {
				DrawSharedToggle (variable);
			}
			GUILayout.EndHorizontal ();
		}

		public virtual void DrawVariableValue (Variable variable, GUIContent label)
		{
			Color color = GUI.backgroundColor;
			if (variable.RawValue == null && !AttributeUtility.HasAttribute (fieldInfo, typeof(NotRequiredAttribute))) {
				GUI.backgroundColor = Color.red;
			}
			EditorGUI.BeginChangeCheck ();
			object obj = null;
			if (variable is BoolVariable) {
				obj = EditorGUILayout.Toggle (label, (bool)variable.RawValue);
			} else if (variable is StringVariable) {
				obj = EditorGUILayout.TextField (label, (string)variable.RawValue);
			} else if (variable is FloatVariable) {
				obj = EditorGUILayout.FloatField (label, (float)variable.RawValue);
			} else if (variable is IntVariable) {
				obj = EditorGUILayout.IntField (label, (int)variable.RawValue);
			} else if (variable is GameObjectVariable) {
				obj = (GameObject)EditorGUILayout.ObjectField (label, (GameObject)variable.RawValue, typeof(GameObject), true);
			} else if (variable is MaterialVariable) {
				obj = (Material)EditorGUILayout.ObjectField (label, (Material)variable.RawValue, typeof(Material), false);
			} else if (variable is ColorVariable) {
				obj = EditorGUILayout.ColorField (label, (Color)variable.RawValue);
			} else if (variable is ObjectVariable) {
				obj = (UnityEngine.Object)EditorGUILayout.ObjectField (label, (UnityEngine.Object)variable.RawValue, typeof(UnityEngine.Object), false);
			} else if (variable is TransformVariable) {
				obj = (Transform)EditorGUILayout.ObjectField (label, (Transform)variable.RawValue, typeof(Transform), true);
			} else if (variable is Vector2Variable) {
				obj = EditorGUILayout.Vector2Field (label, (Vector2)variable.RawValue);
			} else if (variable is Vector3Variable) {
				obj = EditorGUILayout.Vector3Field (label, (Vector3)variable.RawValue);
			} else if (variable is Vector4Variable) {
				obj = EditorGUILayout.Vector4Field (label, (Vector4)variable.RawValue);
			} else if (variable is SpriteVariable) {
				obj = (Sprite)EditorGUILayout.ObjectField (label, (Sprite)variable.RawValue, typeof(Sprite), false);
			}

			/*else if (variable is GenericVariable) {
				GUILayout.BeginVertical ();
				GenericVariable genericVariable = variable as GenericVariable;
				genericVariable.variableType = (VariableType)EditorGUILayout.EnumPopup ("Type", genericVariable.variableType);
				if (genericVariable.variableType != VariableType.None) {
					DrawVariableValue (genericVariable.sourceVariable, label);
				}
				GUI.backgroundColor = color;
				GUILayout.EndVertical ();
				return;
			}*/
				
			if (EditorGUI.EndChangeCheck ()) {
				BehaviorSelection.RecordUndo ("Inspector");
				variable.RawValue = obj;
			}
			GUI.backgroundColor = color;
		}


		public virtual void DrawSharedVariable (Variable variable, GUIContent label)
		{
			if (BehaviorSelection.activeBehaviorTree == null) {
				return;
			}
			Color color = GUI.backgroundColor;
			if (variable.isNone && !AttributeUtility.HasAttribute (fieldInfo, typeof(NotRequiredAttribute))) {
				GUI.backgroundColor = Color.red;
			}
			GUIContent[] names = null;
			int variablesOfType = GetVariablesOfType (variable, out names);
			EditorGUI.BeginChangeCheck ();
			variablesOfType = EditorGUILayout.Popup (label, variablesOfType, names);
			if (EditorGUI.EndChangeCheck ()) {
				BehaviorSelection.RecordUndo ("Inspector");
				variable.name = names [variablesOfType].text;
			}
			GUI.backgroundColor = color;
		}

		public virtual bool DrawSharedToggle (Variable variable)
		{
			EditorGUI.BeginChangeCheck ();
			bool value = EditorGUILayout.Toggle (variable.isShared, EditorStyles.radioButton, GUILayout.Width (17f));
			if (EditorGUI.EndChangeCheck ()) {
				BehaviorSelection.RecordUndo ("Inspector");
				variable.isShared = value;
			}
			return value;
		}


		public int GetVariablesOfType (Variable variable, out GUIContent[] names)
		{
			Variable[] variables = BehaviorSelection.activeBehaviorTree.blackboard.GetVariablesOfType (variable.GetType (), true);
			int count = 0;
			List<GUIContent> strs = new List<GUIContent> () {
				new GUIContent ("None")
			};

			for (int i = 0; i < variables.Length; i++) {
				Type propertyType = variables [i].type;
				if (variable == null || propertyType.Equals (variable.type)) {
					strs.Add (new GUIContent (variables [i].name));
					if (variable != null && variables [i].name.Equals (variable.name)) {
						count = strs.Count - 1;
					}
				}
			}
			names = strs.ToArray ();
			return count;
		}
	}
}