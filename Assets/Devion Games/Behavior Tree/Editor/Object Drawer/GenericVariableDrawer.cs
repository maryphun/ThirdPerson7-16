using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace DevionGames.BehaviorTrees
{
	[CustomObjectDrawer (typeof(GenericVariable))]
	public class GenericVariableDrawer : VariableDrawer
	{
		public override void OnGUI (GUIContent label)
		{
			GenericVariable genericVariable = value as GenericVariable;
			bool sharedOnly = AttributeUtility.HasAttribute (fieldInfo, typeof(SharedAttribute));
		
			genericVariable.variableType = (VariableType)EditorGUILayout.EnumPopup (label, genericVariable.variableType);
			if (genericVariable.variableType != VariableType.None) {
				if (genericVariable.sourceVariable == null) {
					genericVariable.sourceVariable = (Variable)System.Activator.CreateInstance (genericVariable.GetVariableSourceType ());
				}
				GUILayout.BeginHorizontal ();
				label = new GUIContent (label.text, fieldInfo.GetTooltip ());
				if (sharedOnly) {
					genericVariable.sourceVariable.isShared = true;
				}
				if (genericVariable.sourceVariable.isShared) {
					DrawSharedVariable (genericVariable.sourceVariable, new GUIContent ("Value"));
				} else {
					DrawVariableValue (genericVariable.sourceVariable, new GUIContent ("Value"));
				}

				if (!sharedOnly) {
					DrawSharedToggle (genericVariable.sourceVariable);
				}
				GUILayout.EndHorizontal ();
			}
		}

		/*public override void DrawSharedVariable (Variable variable, GUIContent label)
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
				Variable instance = BehaviorSelection.activeBehaviorTree.blackboard.GetVariable (names [variablesOfType].text, true);
				FieldInfo sourceField = this.value.GetType ().GetField ("m_" + (this.value as GenericVariable).variableType.ToString () + "Value");
				if (instance == null) {
					instance = (Variable)System.Activator.CreateInstance (sourceField.FieldType);
					instance.isShared = true;
				}
				sourceField.SetValue (this.value, instance);
			}
			GUI.backgroundColor = color;
		}

		public override bool DrawSharedToggle (Variable variable)
		{
			EditorGUI.BeginChangeCheck ();
			bool value = EditorGUILayout.Toggle (variable.isShared, EditorStyles.radioButton, GUILayout.Width (17f));
			if (EditorGUI.EndChangeCheck ()) {
				FieldInfo sourceField = this.value.GetType ().GetField ("m_" + (this.value as GenericVariable).variableType.ToString () + "Value");
				Variable instance = (Variable)System.Activator.CreateInstance (sourceField.FieldType);
				instance.isShared = value;
				sourceField.SetValue (this.value, instance);
			}
			return value;
		}*/
	}
}