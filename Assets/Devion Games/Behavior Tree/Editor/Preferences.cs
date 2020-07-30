using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DevionGames.BehaviorTrees
{
	public static class Preferences
	{

		private static Dictionary<Preference, bool> preferencesLookup;

		public static void OnGUI ()
		{
			//DrawPreference (Preference.ShowWelcomeWindow, "Show welcome window on start?", true);
			DrawPreference (Preference.ShowBehaviorDescription, "Show behavior description?", true);
			DrawPreference (Preference.OpenInspectorOnTaskClick, "Open inspector on task click?", false);
			DrawPreference (Preference.OpenInspectorOnTaskDoubleClick, "Open inspector on task double click?", true);
			DrawPreference (Preference.ShowSelectedTaskDescription, "Show selected task description?", true);
			DrawPreference (Preference.ShowTaskCommentAsTooltip, "Show task comment as tooltip?", true);
			DrawPreference (Preference.ShowIconInHierarchy, "Show behavior icons in hierarchy?", true);
			DrawPreference (Preference.ShowErrorInHierarchy, "Show error icons in hierarchy?", true);
			DrawPreference (Preference.MoveChildrenOnDrag, "Move children when task is dragged?", true);
			DrawPreference (Preference.HideSidebarOnDrag, "Hide sidebar when task is dragged?", false);
			DrawPreference (Preference.ReorderChildrenByPosition, "Reorder children by position?", true);
			DrawPreference (Preference.DrawInspectorOnDrag, "Draw inspector on drag?", false);
			DrawEnumPreference (Preference.ConnectionStyle, "Connection Style", ConnectionStyle.Curvy);
		}

		private static void DrawEnumPreference<T> (Preference preference, string label, T defaultValue) where T:struct
		{
			GUILayout.BeginHorizontal ();
			int state = EditorPrefs.GetInt (preference.ToString (), (int)(object)defaultValue);
			GUILayout.Label (label);
			int state2 = (int)(object)EditorGUILayout.EnumPopup (GUIContent.none, (Enum)Enum.ToObject (typeof(T), state), GUILayout.Width (100));
			EditorPrefs.SetInt (preference.ToString (), state2);
			GUILayout.EndHorizontal ();
		}



		private static void DrawPreference (Preference preference, string label, bool defaultValue)
		{
			GUILayout.BeginHorizontal ();
			bool state = GetBool (preference, defaultValue);
			GUILayout.Label (label, Styles.wrappedLabel);
			bool state2 = EditorGUILayout.Toggle (GUIContent.none, state, GUILayout.Width (18));
			SetBool (preference, state2);

			GUILayout.EndHorizontal ();
		}

		public static T GetEnum<T> (Preference preference)
		{
			return (T)(object)EditorPrefs.GetInt (preference.ToString ());
		}

		public static bool GetBool (Preference preference)
		{
			if (preferencesLookup == null) {
				preferencesLookup = new Dictionary<Preference, bool> ();			
			}

			bool value;
			if (!Preferences.preferencesLookup.TryGetValue (preference, out value)) {
				value = EditorPrefs.GetBool (preference.ToString ());
				Preferences.preferencesLookup.Add (preference, value);
			}

			return value;
		}

		public static bool ToggleBool (Preference preference)
		{
			bool state = Preferences.GetBool (preference);
			Preferences.SetBool (preference, !state);
			return !state;

		}

		public static bool GetBool (Preference preference, bool defaultValue)
		{

			return EditorPrefs.GetBool (preference.ToString (), defaultValue);
		}

		public static void SetBool (Preference preference, bool state)
		{
			if (preferencesLookup == null) {
				preferencesLookup = new Dictionary<Preference, bool> ();			
			}
			if (preferencesLookup.ContainsKey (preference)) {
				preferencesLookup [preference] = state;			
			}
			EditorPrefs.SetBool (preference.ToString (), state);
		}
	}

	public enum Preference
	{
		ShowWelcomeWindow,
		OpenInspectorOnTaskClick,
		OpenInspectorOnTaskDoubleClick,
		ShowSelectedTaskDescription,
		ShowTaskCommentAsTooltip,
		ShowIconInHierarchy,
		ShowErrorInHierarchy,
		ShowBehaviorDescription,
		MoveChildrenOnDrag,
		HideSidebarOnDrag,
		ReorderChildrenByPosition,
		DrawInspectorOnDrag,
		ConnectionStyle
	}
}