using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;
using System.Reflection;

namespace DevionGames.BehaviorTrees
{
	public class GlobalVariablesWindow : EditorWindow
	{
		[MenuItem ("Tools/Devion Games/Behavior Trees/Global Variables", false, 1)]
		public static void ShowWindow ()
		{
			GlobalVariablesWindow window = EditorWindow.GetWindow<GlobalVariablesWindow> (false, "Global Variables");
			window.minSize = new Vector2 (250f, 300f);
			window.wantsMouseMove = true;
		}

		private GlobalVariables m_GlobalVariables;
		private Vector2 m_ScrollPosition;

		private void Update ()
		{
			if (this.m_GlobalVariables == null && !EditorApplication.isCompiling) {
				this.m_GlobalVariables = Resources.Load<GlobalVariables> ("GlobalVariables");
				if (this.m_GlobalVariables == null) {
					if (!System.IO.Directory.Exists (Application.dataPath + "/Resources")) {
						AssetDatabase.CreateFolder ("Assets", "Resources");
					}	
					this.m_GlobalVariables = AssetCreator.CreateAsset<GlobalVariables> ("Assets/Resources/GlobalVariables.asset");
					EditorUtility.DisplayDialog ("Global Variables Created!", "Do not rename the Resource folder and the GlobalVariables asset! You can move the Resources folder to any location in your project.", "Ok");
				}
				this.Repaint ();
			}
		}

		private void OnFocus ()
		{
			this.Repaint ();
		}

		private ReorderableList m_VariableList;
		private string m_SearchString = "Search...";
		private int m_RenameIndex = -1;

		public void OnGUI ()
		{
			if (this.m_GlobalVariables == null) {
				return;
			}
			if (this.m_VariableList == null) {
				CreateVariableList ();
			}
			if (this.m_VariableList != null) {
				this.DrawVariableToolbar ();
				this.m_ScrollPosition = GUILayout.BeginScrollView (this.m_ScrollPosition);
				this.m_VariableList.DoLayoutList ();
				GUILayout.EndScrollView ();
			}
		}

		private void DrawVariableToolbar ()
		{
			GUILayout.BeginHorizontal (EditorStyles.toolbar);

			if (Search ()) {
				CreateVariableList ();
			}

			GUIStyle style = new GUIStyle("ToolbarCreateAddNewDropDown");
			GUIContent content = EditorGUIUtility.IconContent("CreateAddNew");

			if (GUILayout.Button(content, style, GUILayout.Width(35f)))
			{
				AddVariable();
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (2f);
		}

		private void AddVariable ()
		{
			GenericMenu menu = new GenericMenu ();
			Type[] nodeTypes = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (assembly => assembly.GetTypes ()).Where (type => type.IsSubclassOf (typeof(Variable)) && !typeof(GenericVariable).IsAssignableFrom (type) && !type.IsAbstract).ToArray ();
			for (int i = 0; i < nodeTypes.Length; i++) {
				Type type = nodeTypes [i];
				menu.AddItem (new GUIContent (type.Name.Replace ("Variable", "")), false, delegate() {
					Variable variable = (Variable)Activator.CreateInstance (type); 
					variable.name = "New " + type.Name.Replace ("Variable", "");
					variable.index = this.m_VariableList.count;
					variable.isShared = true;
					this.m_GlobalVariables.Add (variable);
					EditorUtility.SetDirty (this.m_GlobalVariables);
					CreateVariableList ();
				});

			}

			menu.ShowAsContext ();
		}

		private void CreateVariableList ()
		{
			Variable[] variables = this.m_GlobalVariables.GetAllVariables ();
			List<Variable> searchedVariables = new List<Variable> ();
			if (this.m_SearchString != "Search...") {
				for (int i = 0; i < variables.Length; i++) {
					if (variables [i].name.ToLower ().StartsWith (this.m_SearchString.ToLower ())) {
						searchedVariables.Add (variables [i]);
					}
				}
			} else {
				searchedVariables.AddRange (variables);
			}
			this.m_VariableList = new ReorderableList (searchedVariables.ToArray (), typeof(Variable), true, false, false, false) {
				onReorderCallback = new ReorderableList.ReorderCallbackDelegate (this.OnReorder),
				drawElementCallback = new ReorderableList.ElementCallbackDelegate (this.OnDrawVariable),
				onSelectCallback = new ReorderableList.SelectCallbackDelegate (this.OnSelect),
				index = 0,
				headerHeight = 0f,
				elementHeight = 24f
			};
		}

		private void OnDrawVariable (Rect rect, int index, bool selected, bool focused)
		{
			Event currentEvent = Event.current;
			if (currentEvent.type == EventType.MouseUp && currentEvent.button == 1 && rect.Contains (currentEvent.mousePosition)) {
				GenericMenu genericMenu = new GenericMenu ();

				genericMenu.AddItem (new GUIContent ("Delete"), false, delegate() {
					this.DeleteVariable (index);
				});
				genericMenu.ShowAsContext ();
				Event.current.Use ();
			}

			if (index < this.m_VariableList.list.Count) {
				EditorGUI.BeginChangeCheck ();
				Variable item = this.m_VariableList.list [index] as Variable;
				rect.yMin = rect.yMin + 2f;
				rect.yMax = rect.yMax - 3f;
				Rect rect1 = rect;	
				rect1.width = 95f;
				Rect rect2 = new Rect (rect1.x + rect1.width + 2f, rect1.y + 2, rect.width - 99f, EditorGUIUtility.singleLineHeight);

				switch (currentEvent.rawType) {
				case EventType.MouseDown:
					if (rect1.Contains (currentEvent.mousePosition) && index == this.m_VariableList.index && currentEvent.button == 0 && currentEvent.type == EventType.MouseDown) {
						this.m_RenameIndex = index;
						GUI.FocusControl ("");
					} 
					break;
				case EventType.KeyDown:
					if (currentEvent.keyCode == KeyCode.Return && this.m_RenameIndex != -1) {
						this.m_RenameIndex = -1;
						currentEvent.Use ();
					}
					break;
				case EventType.MouseUp:
					if (!rect1.Contains (Event.current.mousePosition) && Event.current.clickCount > 0 && index == this.m_VariableList.index && this.m_RenameIndex != -1) {
						this.m_RenameIndex = -1;
						Event.current.Use ();
					}
					break;
				}

				if (index == this.m_RenameIndex) {
					string before = item.name;
					string after = EditorGUI.TextField (rect1, item.name);
					if (before != after) {
						item.name = after;
						UpdateNodeVariables (before, after);
					}
				} else {
					GUI.Label (rect1, item.name);
				}
				if (item is BoolVariable) {
					item.RawValue = EditorGUI.Toggle (rect2, (bool)item.RawValue);
				} else if (item is StringVariable) {
					item.RawValue = EditorGUI.TextField (rect2, (string)item.RawValue);
				} else if (item is FloatVariable) {
					item.RawValue = EditorGUI.FloatField (rect2, (float)item.RawValue);
				} else if (item is IntVariable) {
					item.RawValue = EditorGUI.IntField (rect2, (int)item.RawValue);
				} else if (item is GameObjectVariable) {
					item.RawValue = (GameObject)EditorGUI.ObjectField (rect2, (GameObject)item.RawValue, typeof(GameObject), true);
				} else if (item is MaterialVariable) {
					item.RawValue = (Material)EditorGUI.ObjectField (rect2, (Material)item.RawValue, typeof(Material), false);
				} else if (item is ColorVariable) {
					item.RawValue = EditorGUI.ColorField (rect2, (Color)item.RawValue);
				} else if (item is ObjectVariable) {
					item.RawValue = (UnityEngine.Object)EditorGUI.ObjectField (rect2, (UnityEngine.Object)item.RawValue, typeof(UnityEngine.Object), false);
				} else if (item is TransformVariable) {
					item.RawValue = (Transform)EditorGUI.ObjectField (rect2, (Transform)item.RawValue, typeof(Transform), false);
				} else if (item is Vector2Variable) {
					item.RawValue = EditorGUI.Vector2Field (rect2, GUIContent.none, (Vector2)item.RawValue);
				} else if (item is Vector3Variable) {
					item.RawValue = EditorGUI.Vector3Field (rect2, GUIContent.none, (Vector3)item.RawValue);
				} else if (item is Vector4Variable) {
					item.RawValue = EditorGUI.Vector4Field (rect2, GUIContent.none, (Vector4)item.RawValue);
				} else if (item is SpriteVariable) {
					item.RawValue = (Sprite)EditorGUI.ObjectField (rect2, GUIContent.none, (Sprite)item.RawValue, typeof(UnityEngine.Sprite), false);
				} 

				if (EditorGUI.EndChangeCheck ()) {
					EditorUtility.SetDirty (this.m_GlobalVariables);
				}
			}
		}

		private void UpdateNodeVariables (string name, string newName)
		{
			if (BehaviorSelection.activeBehavior != null) {
				Task[] nodes =	BehaviorUtility.GetAllNodes (BehaviorSelection.activeBehavior);
				for (int i = 0; i < nodes.Length; i++) {
					Task node = nodes [i];
					FieldInfo[] fields = node.GetType ().GetFields ();
					for (int f = 0; f < fields.Length; f++) {
						object obj = fields [f].GetValue (node);
						if (obj is Variable) {
							Variable variable = obj as Variable;
							if (variable.isShared && variable.name == name) {
								variable.name = newName;
							}
						}
					}
				}
			}
		}

		private void OnSelect (ReorderableList list)
		{
			if (this.m_RenameIndex != list.index) {
				this.m_RenameIndex = -1;
			}
		}

		private void OnReorder (ReorderableList list)
		{
			this.m_GlobalVariables.Clear ();
			Variable[] variables = list.list.Cast<Variable> ().ToArray ();
			for (int i = 0; i < variables.Length; i++) {
				variables [i].index = i;
			}
			this.m_GlobalVariables.AddRange (variables);
			EditorUtility.SetDirty (this.m_GlobalVariables);
		}

		private void DeleteVariable (int index)
		{
			Variable item = this.m_VariableList.list [index] as Variable;
			this.m_GlobalVariables.Remove (item);
			CreateVariableList ();
			EditorUtility.SetDirty (this.m_GlobalVariables);
		}

		private bool Search()
		{
			bool changed;
			this.m_SearchString = SearchField(this.m_SearchString, out changed, GUILayout.MinWidth(155f));
			return changed;
		}

		private string SearchField(string search, out bool changed, params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginHorizontal();
			changed = false;
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
				changed = true;
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
			if (before != after)
			{
				changed = true;
			}
			return after;
		}

	}
}