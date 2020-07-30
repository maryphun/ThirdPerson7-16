using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace DevionGames.BehaviorTrees
{

	[System.Serializable]
	public class GraphInspector<T> where T : IEnumerable, IEnabled
	{
		public void OnGUI (Graph<T> graph, Rect position)
		{
			List<T> selection = graph.m_Selection;
			for (int i = 0; i < selection.Count; i++) {
				T node = selection [i];
				string hash = node.GetHashCode ().ToString ();
				bool foldout = EditorPrefs.GetBool (hash, true);

				bool state = NodeTitlebar (node, foldout);
				if (state != foldout) {
					EditorPrefs.SetBool (hash, state);
				}
				if (state) {
					DrawerUtility.BeginIndent (1);
					EditorGUIUtility.labelWidth = 110f;
					bool changed;
					DrawerUtility.DrawFields (node, out changed);
					if (changed) {
                        BehaviorSelection.SetDirty();
					}
					EditorGUIUtility.labelWidth = 0f;
					DrawerUtility.EndIndent ();
				}
			}
		}

		private static GenericMenu GetSettingsMenu(T node) {
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Edit Script"), false, delegate
			{
				MonoScript script = FindMonoScript(node.GetType());
				AssetDatabase.OpenAsset(script);

			});

			menu.AddItem(new GUIContent("Locate Script"), false, delegate
			{
				MonoScript script = FindMonoScript(node.GetType());		
				Selection.activeObject = script;
			});
			return menu;
		}

		private static MonoScript FindMonoScript(Type type){
			string[] assetPaths = AssetDatabase.GetAllAssetPaths();
			foreach (string assetPath in assetPaths)
			{
				if (assetPath.EndsWith(".cs"))
				{

					MonoScript m = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
					if (m.GetClass() != null && m.GetClass() == type)
					{
						return m;

					}
				}
			}
			return null;
		}

		public static bool NodeTitlebar (T node, bool foldout)
		{
			int controlID = EditorGUIUtility.GetControlID (FocusType.Passive);
			string s = node.GetCategory ().Substring (node.GetCategory ().LastIndexOf ('/') + 1);
			GUIContent content = new GUIContent ((string.IsNullOrEmpty (s) ? "" : s + ".") + node.GetType ().Name, node.GetTooltip ());
			Rect position = GUILayoutUtility.GetRect (GUIContent.none, Styles.inspectorTitle);
			position.x += 1f;
			position.width -= 3f;
			Rect rect = new Rect (position.x + (float)Styles.inspectorTitle.padding.left, position.y + (float)Styles.inspectorTitle.padding.top, 16f, 16f);
			Rect rect1 = new Rect (position.xMax - (float)Styles.inspectorTitle.padding.right - 10f, rect.y, 16f, 16f);
			Rect rect4 = rect1;
			rect4.x = rect4.x - 18f;

			Rect rect2 = new Rect (position.x + 2f + 2f + 16f * 2, rect.y, 100f, rect.height) {
				xMax = rect4.xMin - 2f
			};
			Rect rect3 = new Rect (position.x + 16f, rect.y, 16f, 16f);
			EditorGUI.BeginChangeCheck ();
			bool toggle = GUI.Toggle (rect3, node.enabled, GUIContent.none);
			if (EditorGUI.EndChangeCheck ()) {
				BehaviorSelection.RecordUndo ("Inspector");
				node.enabled = toggle;
			}
			string url = node.GetHelpUrl ();
			if (GUI.Button (rect1, Styles.popupIcon, Styles.inspectorTitleText)) {
				GetSettingsMenu(node).ShowAsContext ();
			}

			if (!string.IsNullOrEmpty (url) && GUI.Button (rect4, Styles.helpIcon, Styles.inspectorTitleText)) {
				Application.OpenURL (url);
			}

			EventType eventType = Event.current.type;
			if (eventType != EventType.MouseDown) {
				if (eventType == EventType.Repaint) {
					Styles.inspectorTitle.Draw (position, GUIContent.none, controlID, foldout);
					Styles.inspectorTitleText.Draw (rect2, content, controlID, foldout);	
				}
			}
			position.width = 15;

			bool flag = DrawerUtility.DoToggleForward (position, controlID, foldout, GUIContent.none, GUIStyle.none);

			return flag;
		}
			
	}
}