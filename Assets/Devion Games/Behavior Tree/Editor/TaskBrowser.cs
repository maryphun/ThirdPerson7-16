using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using DevionGames.BehaviorTrees.Actions;
using DevionGames.BehaviorTrees.Conditionals;

namespace DevionGames.BehaviorTrees
{
	public class TaskBrowser : EditorWindow
	{
		private static string m_SearchString="Search...";
		private static Vector2 m_ScrollPosition;
		public static Task active;
		private static double clickTime;
		private static double doubleClickTime = 0.3;
		private static TaskElement treeView;

		[MenuItem ("Tools/Devion Games/Behavior Trees/Task Browser", false, 4)]
		public static void ShowWindow ()
		{
			TaskBrowser window = EditorWindow.GetWindow<TaskBrowser> ("Tasks");
			Vector2 size = new Vector2 (250f, 250f);
			window.minSize = size;
		}

		private void OnEnable ()
		{
			treeView = BuildTaskElements ();
		}

		private void OnGUI ()
		{
			DoOnGUI ();
		}

		public static void DoOnGUI ()
		{
			if (treeView == null) {
				treeView = BuildTaskElements ();
			}
			m_SearchString = SearchField (m_SearchString);
			GUILayout.Space (2.0f);
			m_ScrollPosition = GUILayout.BeginScrollView (m_ScrollPosition);
			if (!string.IsNullOrEmpty (m_SearchString) && m_SearchString != "Search...") {
				foreach (TaskElement view in GetAllElements()) {
					if (view.label.text.ToLower ().StartsWith (m_SearchString.ToLower ())) {
						EditorGUILayout.GetControlRect (true, 16f, EditorStyles.label);
						Rect rect = GUILayoutUtility.GetLastRect ();
						rect.x = EditorGUI.indentLevel * 15f + 4f;
						GUIStyle style = new GUIStyle ("label");
						if (active != null && active.GetType () == view.type) {
							style.normal.textColor = EditorStyles.foldout.focused.textColor;
						}
						if (GUI.Button (rect, view.path + "." + view.label.text, style)) {
							TaskBrowser.active = (Task)Activator.CreateInstance (view.type);
							TaskBrowser.focusedWindow.Repaint ();
							if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime) {
								if (BehaviorSelection.activeBehaviorTree != null && BehaviorTreeWindow.current != null) {

									BehaviorUtility.AddNode (BehaviorSelection.activeBehaviorTree, null, active.GetType (), -BehaviorTreeWindow.current.m_Graph.m_GraphOffset + BehaviorTreeWindow.current.m_Graph.m_GraphArea.size * 0.5f / BehaviorTreeWindow.current.m_Graph.m_GraphZoom);
                                    BehaviorUtility.Save(BehaviorSelection.activeBehaviorTree);
                                    BehaviorSelection.SetDirty();
                                    BehaviorTreeWindow.current.Repaint ();
								}
							}
							clickTime = EditorApplication.timeSinceStartup;
						}
					}
				}
			} else {
				foreach (TaskElement view in treeView.children) {
					view.OnGUI ();
				}
			}
			GUILayout.EndScrollView ();
			if (active != null) {
				GUILayout.BeginVertical ();
				GUIStyle style = new GUIStyle ("IN BigTitle");
				style.padding.top = 0;
				GUILayout.BeginVertical (style);
				GUILayout.BeginHorizontal ();
				GUILayout.Label (active.name, EditorStyles.boldLabel);
				GUILayout.FlexibleSpace ();
				GUIStyle labelStyle = new GUIStyle ("label");
				labelStyle.contentOffset = new Vector2 (0, 5);
				if (!string.IsNullOrEmpty (active.GetHelpUrl ()) && GUILayout.Button (Styles.helpIcon, labelStyle, GUILayout.Width (20))) {
					Application.OpenURL (active.GetHelpUrl ());
				}
				GUILayout.EndHorizontal ();
				GUILayout.Space (3f);
				GUILayout.Label (active.GetTooltip (), Styles.wrappedLabel);
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();

				if (GUILayout.Button ("Add task")) {
					if (BehaviorSelection.activeBehaviorTree != null && BehaviorTreeWindow.current != null) {
						BehaviorUtility.AddNode (BehaviorSelection.activeBehaviorTree, null, active.GetType (), -BehaviorTreeWindow.current.m_Graph.m_GraphOffset + BehaviorTreeWindow.current.m_Graph.m_GraphArea.size * 0.5f / BehaviorTreeWindow.current.m_Graph.m_GraphZoom);
                        BehaviorUtility.Save(BehaviorSelection.activeBehaviorTree);
                        BehaviorSelection.SetDirty();
                        BehaviorTreeWindow.current.Repaint ();
					}
				}

				GUILayout.EndHorizontal ();
				GUILayout.EndVertical ();

				EditorGUI.BeginDisabledGroup (true);
				GraphInspector<Task>.NodeTitlebar (active, true);
				bool changed;
				DrawerUtility.DrawFields (active, out changed);
				EditorGUI.EndDisabledGroup ();
				GUILayout.Space (5);
				GUILayout.EndVertical ();
			}
		}

		private static TaskElement[] GetAllElements ()
		{
			List<TaskElement> elements = new List<TaskElement> ();
			GetTaskElements (treeView, ref elements);
			return elements.ToArray ();
		}

		private static void GetTaskElements (TaskElement current, ref List<TaskElement> list)
		{
			list.Add (current);
			for (int i = 0; i < current.children.Count; i++) {
				GetTaskElements (current.children [i], ref list);
			}
		}

		private static TaskElement BuildTaskElements ()
		{
			TaskElement root = new TaskElement ("Root", "");
			Type[] types = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (assembly => assembly.GetTypes ()).Where (type => typeof(Task).IsAssignableFrom (type) && !type.IsAbstract && type != typeof(EntryTask)).ToArray ();
			types = types.OrderBy (x => x.BaseType.Name).ToArray ();
			foreach (Type type in types) {
				string baseType = type.BaseType.Name + "s";
				string category = type.GetCategory ();
				string menu = baseType + (string.IsNullOrEmpty (category) ? "" : "/" + category);
				menu = menu.Replace ("/", ".");

				string[] s = menu.Split ('.');
				TaskElement prev = null;
				string cur = string.Empty;
				for (int i = 0; i < s.Length; i++) {
					cur += (string.IsNullOrEmpty (cur) ? "" : ".") + s [i];
					TaskElement parent = root.Find (cur);
					if (parent == null) {
						parent = new TaskElement (s [i], cur);
						if (prev != null) {
							prev.children.Add (parent);
						} else {
							root.children.Add (parent);
						}
					}
	
					prev = parent;

				}
				TaskElement element = new TaskElement (type.Name, menu);
				element.type = type;
				prev.children.Add (element);
			}
			return root;
		}


		private static string SearchField(string search, params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginHorizontal();
			string before = search;

			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, "ToolbarSeachTextField", options);
			Rect buttonRect = rect;
			buttonRect.x = rect.width - 14;
			buttonRect.width = 14;

			if (!String.IsNullOrEmpty(before))
				EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Arrow);

			if (Event.current.type == EventType.MouseUp && buttonRect.Contains(Event.current.mousePosition) || before == "Search..." && GUI.GetNameOfFocusedControl() == "SearchTextFieldFocus")
			{
				before = "";
				GUI.changed = true;
				GUI.FocusControl(null);

			}
			GUI.SetNextControlName("SearchTextFieldFocus");
			GUIStyle style = new GUIStyle("ToolbarSeachTextField");
			if (before == "Search...")
			{
				style.normal.textColor = Color.gray;
				style.hover.textColor = Color.gray;
			}
			string after = EditorGUI.TextField(rect, "", before, style);

			if (GUI.changed)
			{
				EditorGUI.FocusTextInControl("SearchTextFieldFocus");
			}
			else if (string.IsNullOrEmpty(after) && GUI.GetNameOfFocusedControl() != "SearchTextFieldFocus")
			{
				after = before = "Search...";
			}

			GUI.Button(buttonRect, GUIContent.none, (after != "" && after != "Search...") ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");
			EditorGUILayout.EndHorizontal();
			return after;
		}
		/*private static string SearchField (string search, params GUILayoutOption[] options)
		{
			GUILayout.BeginHorizontal ();
			string before = search;
			string after = EditorGUILayout.TextField ("", before, "SearchTextField");

			if (GUILayout.Button ("", "SearchCancelButton", GUILayout.Width (18f))) {
				after = string.Empty;
				GUIUtility.keyboardControl = 0;
			}
			GUILayout.EndHorizontal ();
			return after;
		}*/
		
		public class TaskElement
		{

			public Type type;

			private string m_Path;

			public string path {
				get { 
					return this.m_Path;
				}
			}

			public bool foldout {
				get {
					return EditorPrefs.GetBool (this.m_Path);
				}	
				set {
					EditorPrefs.SetBool (this.m_Path, value);
				}
			}

			private GUIContent m_Label;

			public GUIContent label {
				get {
					return this.m_Label;
				}
				set { 
					this.m_Label = value;
				}
			}

			public TaskElement (string label, string path)
			{
				this.label = new GUIContent (label);
				this.m_Path = path;
			}

			private List<TaskElement> m_children;

			public List<TaskElement> children {
				get { 
					if (this.m_children == null) {
						this.m_children = new List<TaskElement> ();
					}
					return m_children;
				}
				set {
					this.m_children = value;
				}
			}

			public bool Contains (TaskElement item)
			{
				if (item.label.text == label.text) {
					return true;
				}
				for (int i = 0; i < children.Count; i++) {
					bool contains = children [i].Contains (item);
					if (contains) {
						return true;
					}
				}
				return false;
			}

			public TaskElement Find (string path)
			{
				if (this.path == path) {
					return this;
				}
				for (int i = 0; i < children.Count; i++) {
					TaskElement tree = children [i].Find (path);
					if (tree != null) {
						return tree;
					}
				}
				return null;
			}

			public void OnGUI ()
			{
				if (children.Count > 0) {
					EditorGUILayout.GetControlRect (true, 16f, EditorStyles.foldout);
					Rect foldRect = GUILayoutUtility.GetLastRect ();
					GUIStyle style = new GUIStyle (EditorStyles.foldout);
					style.focused.textColor = style.normal.textColor;
					style.onFocused.textColor = style.normal.textColor;
					style.active.textColor = style.normal.textColor;
					style.hover.textColor = style.normal.textColor;

					foldout = EditorGUI.Foldout (foldRect, foldout, label, style);

					if (foldout) {
						EditorGUI.indentLevel += 1;
						for (int i = 0; i < children.Count; i++) {
							children [i].OnGUI ();
						}
						EditorGUI.indentLevel -= 1;
					}

				} else {
					EditorGUILayout.GetControlRect (true, 16f, EditorStyles.label);
					Rect rect = GUILayoutUtility.GetLastRect ();
					rect.x = EditorGUI.indentLevel * 15f + 4f;
					GUIStyle style = new GUIStyle ("label");
					if (active != null && active.GetType () == type) {
						style.normal.textColor = EditorStyles.foldout.focused.textColor;
					}
	
					if (GUI.Button (rect, label, style)) {
						TaskBrowser.active = (Task)Activator.CreateInstance (type);
						TaskBrowser.focusedWindow.Repaint ();
						if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime) {
							if (BehaviorSelection.activeBehaviorTree != null && BehaviorTreeWindow.current != null) {
				
								BehaviorUtility.AddNode (BehaviorSelection.activeBehaviorTree, null, active.GetType (), -BehaviorTreeWindow.current.m_Graph.m_GraphOffset + BehaviorTreeWindow.current.m_Graph.m_GraphArea.size * 0.5f / BehaviorTreeWindow.current.m_Graph.m_GraphZoom);
								BehaviorTreeWindow.current.Repaint ();
							}
						}
						clickTime = EditorApplication.timeSinceStartup;
					}
				}
			}
		}
	}
}