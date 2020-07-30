using UnityEditor;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[CustomPropertyDrawer (typeof(TextAreaAttribute))]
	public class TextAreaDrawer : PropertyDrawer
	{
		private const int kLineHeight = 13;

		public TextAreaDrawer ()
		{
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			TextAreaAttribute textAreaAttribute = base.attribute as TextAreaAttribute;
			string str = property.stringValue;
			float single = EditorStyles.textArea.CalcHeight (new GUIContent (str), EditorGUIUtility.currentViewWidth);
			int num = Mathf.CeilToInt (single / 13f);
			num = Mathf.Clamp (num, textAreaAttribute.minLines, textAreaAttribute.maxLines);
			return 32f + (float)((num - 1) * 13);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.String) {
				EditorGUI.LabelField (position, label.text, "Use TextAreaDrawer with string.");
			} else {
				label = EditorGUI.BeginProperty (position, label, property);
				Rect rect = position;
				rect.height = 16f;
				position.yMin = position.yMin + rect.height;
				EditorGUI.HandlePrefixLabel (position, EditorGUI.IndentedRect (rect), label);
				EditorGUI.BeginChangeCheck ();
				string str = EditorGUI.TextArea (position, property.stringValue, EditorStyles.textArea);
				if (EditorGUI.EndChangeCheck ()) {
					property.stringValue = str;
				}
				EditorGUI.EndProperty ();
			}
		}
	}
}