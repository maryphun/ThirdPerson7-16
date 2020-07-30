using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using UnityEditorInternal;

namespace DevionGames.BehaviorTrees
{
	public static class DrawerUtility
	{
		
		public static object DrawFields (object obj, out bool changed)
		{
			changed = false;
			if (obj == null) {
				return null;
			}
			Type type = obj.GetType ();

			FieldInfo[] fields = type.GetAllFields ().Where (x => !x.HasAttribute (typeof(HideInInspector))).GroupBy(x => x.Name).Select(x => x.First()).ToArray ();
		
			for (int j = 0; j < fields.Length; j++) {
				FieldInfo field = fields [j];
				GUIContent label = new GUIContent (ObjectNames.NicifyVariableName (field.Name), field.GetTooltip ());
				object value = field.GetValue (obj);

				if (value == null) {
					if (Type.GetTypeCode (field.FieldType) == TypeCode.String) {
						value = string.Empty;
					} else if (typeof(IList).IsAssignableFrom (field.FieldType)) {
						value = Activator.CreateInstance (field.FieldType, new object[]{ 0 });
					} else {
						value = System.Activator.CreateInstance (field.FieldType);
					}
					field.SetValue (obj, value);
				}

				EditorGUI.BeginChangeCheck ();

				object val = DrawerUtility.DrawField (label, obj, value, field);
				if (EditorGUI.EndChangeCheck ()) {
					BehaviorSelection.RecordUndo ("Inspector");
					field.SetValue (obj, val);
					changed = true;
				}

			}
			return obj;
		}

		public static object DrawField (GUIContent label, object obj, object value, FieldInfo field, params GUILayoutOption[] options)
		{
			Type type = value.GetType ();
			ObjectDrawer customDrawer = GUIDrawer.GetDrawer (type);
			if (customDrawer != null) {
				customDrawer.declaringObject = obj;
				customDrawer.fieldInfo = field;
				customDrawer.value = value;
				customDrawer.OnGUI (label);
				return value;
			}
			if (type == typeof(int)) {
				return EditorGUILayout.IntField (label, (int)value, options);
			} else if (type == typeof(float)) {
				return EditorGUILayout.FloatField (label, (float)value, options);
			} else if (type == typeof(string)) {
				return EditorGUILayout.TextField (label, (string)value, options);
			} else if (typeof(Enum).IsAssignableFrom (type)) {
				return EditorGUILayout.EnumPopup (label, (Enum)value, options);
			} else if (type == typeof(bool)) {
				return EditorGUILayout.Toggle (label, (bool)value, options);
			} else if (type == typeof(Color)) {
				return EditorGUILayout.ColorField (label, (Color)value, options);
			} else if (type == typeof(Bounds)) {
				return EditorGUILayout.BoundsField (label, (Bounds)value, options);
			} else if (type == typeof(AnimationCurve)) {
				return EditorGUILayout.CurveField (label, (AnimationCurve)value, options);
			} else if (type == typeof(Rect)) {
				return EditorGUILayout.RectField (label, (Rect)value, options);
			} else if (type == typeof(Vector2)) {
				return EditorGUILayout.Vector2Field (label, (Vector2)value, options);
			} else if (type == typeof(Vector3)) {
				return EditorGUILayout.Vector3Field (label, (Vector3)value, options);
			} else if (type == typeof(Vector4)) {
				return EditorGUILayout.Vector4Field (label, (Vector4)value, options);
			} else if (type == typeof(LayerMask)) {
				return LayerMaskField (label, (LayerMask)value, options);
			} else if (typeof(UnityEngine.Object).IsAssignableFrom (type)) {
				return EditorGUILayout.ObjectField (label, (UnityEngine.Object)value, type, true, options);
			} else if (typeof(IList).IsAssignableFrom (type)) {
				if (DrawerUtility.Foldout (type.Name + label.text, label)) {
					DrawerUtility.BeginIndent (1, true);
					Type elementType = GetElementType (type);
					IList list = (IList)value;
					EditorGUI.BeginChangeCheck ();
					int size = EditorGUILayout.IntField ("Size", list.Count);
					size = Mathf.Clamp (size, 0, int.MaxValue);

					if (size != list.Count) {

						Array array = Array.CreateInstance (elementType, size);
						int index = 0;
						while (index < size) {
							object item = null;
							if (index < list.Count) {
								item = list [index];
							} else {
								if (Type.GetTypeCode (elementType) == TypeCode.String) {
									item = string.Empty;
								} else {
									item = Activator.CreateInstance (elementType, true);
								}

							}
							array.SetValue (item, index);
							index++;

						}
						if (type.IsArray) {
							list = array;
						} else {
							list.Clear ();
							for (int i = 0; i < array.Length; i++) {
								list.Add (array.GetValue (i));
							}
						}

					}
						
					for (int i = 0; i < list.Count; i++) {
						list [i] = DrawField (new GUIContent ("Element " + i), list, list [i], field);
					}
					DrawerUtility.EndIndent ();
					return list;
				}
				return value;
			} 
			if (DrawerUtility.Foldout (type.Name + label.text, label)) {
				DrawerUtility.BeginIndent (1, true);
				bool changed;
				value = DrawFields (value, out changed);
				DrawerUtility.EndIndent ();
			}
			return value;
		}

		private static Type GetElementType (Type type)
		{
			Type[] interfaces = type.GetInterfaces ();

			return (from i in interfaces
			        where i.IsGenericType && i.GetGenericTypeDefinition () == typeof(IEnumerable<>)
			        select i.GetGenericArguments () [0]).FirstOrDefault ();
		}

		public static LayerMask LayerMaskField (GUIContent label, LayerMask layerMask, params GUILayoutOption[] options)
		{
			List<string> layers = new List<string> ();
			List<int> layerNumbers = new List<int> ();

			for (int i = 0; i < 32; i++) {
				string layerName = LayerMask.LayerToName (i);
				if (layerName != "") {
					layers.Add (layerName);
					layerNumbers.Add (i);
				}
			}
			int maskWithoutEmpty = 0;
			for (int i = 0; i < layerNumbers.Count; i++) {
				if (((1 << layerNumbers [i]) & layerMask.value) > 0)
					maskWithoutEmpty |= (1 << i);
			}
			maskWithoutEmpty = EditorGUILayout.MaskField (label, maskWithoutEmpty, layers.ToArray (), options);
			int mask = 0;
			for (int i = 0; i < layerNumbers.Count; i++) {
				if ((maskWithoutEmpty & (1 << i)) > 0)
					mask |= (1 << layerNumbers [i]);
			}
			layerMask.value = mask;
			return layerMask;
		}

		public static void BeginIndent (int indent, bool fold = false)
		{
			GUILayout.BeginHorizontal ();

			GUILayout.Space (indent * 15f - (fold ? 2f : 0f));
			GUILayout.BeginVertical ();
		}

		public static void EndIndent ()
		{
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
		
		}

		public static bool Foldout (string hash, GUIContent content)
		{
			return Foldout (hash, content, EditorStyles.foldout);
		}

		public static bool Foldout (string hash, GUIContent content, GUIStyle style)
		{
			bool foldout = EditorPrefs.GetBool (hash, true);

			bool flag = EditorGUILayout.Foldout (foldout, content, style);
			if (flag != foldout) {
				EditorPrefs.SetBool (hash, flag);
			}

			return flag;
		}

		public static bool DoToggleForward (Rect position, int id, bool value, GUIContent content, GUIStyle style)
		{
			Event ev = Event.current;
			if (ev.MainActionKeyForControl (id)) {
				value = !value;
				ev.Use ();
				GUI.changed = true;
			}
			if (EditorGUI.showMixedValue) {
				style = "ToggleMixed";
			}
			EventType eventType = ev.type;
			bool flag = (ev.type != EventType.MouseDown ? false : ev.button != 0);
			if (flag) {
				ev.type = EventType.Ignore;
			}
			bool flag1 = GUI.Toggle (position, id, (!EditorGUI.showMixedValue ? value : false), content, style);
			if (flag) {
				ev.type = eventType;
			} else if (ev.type != eventType) {
				GUIUtility.keyboardControl = id;
			}
			return flag1;
		}

		public static bool MainActionKeyForControl (this Event evt, int controlId)
		{
			if (GUIUtility.keyboardControl != controlId) {
				return false;
			}
			bool flag = (evt.alt || evt.shift || evt.command ? true : evt.control);
			if (evt.type == EventType.KeyDown && evt.character == ' ' && !flag) {
				evt.Use ();
				return false;
			}
			return (evt.type != EventType.KeyDown || evt.keyCode != KeyCode.Space && evt.keyCode != KeyCode.Return && evt.keyCode != KeyCode.KeypadEnter ? false : !flag);
		}
	}
}