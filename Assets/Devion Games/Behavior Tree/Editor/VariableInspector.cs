using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class VariableInspector
	{
		private ReorderableList m_VariableList;
		private ReorderableList m_Empty;
		private string m_SearchString = "Search...";
		private int m_RenameIndex = -1;

		public VariableInspector ()
		{
			BehaviorSelection.selectionChanged += OnSelectionChange;
		}

		public void OnGUI ()
		{
			if (this.m_VariableList == null && BehaviorSelection.activeBehaviorTree != null) {
				CreateVariableList ();
			}

			this.DrawVariableToolbar ();

			if (this.m_VariableList != null) {
				this.m_VariableList.DoLayoutList ();
			} else {
				if (m_Empty == null) {
					m_Empty = new ReorderableList (new List<int> (), typeof(int), false, false, false, false) {
						headerHeight = 0,
						elementHeight = 24	
					};
				}
				this.m_Empty.DoLayoutList ();
			}
		}

		private void OnSelectionChange ()
		{
			this.m_VariableList = null;
		}

		private void DrawVariableToolbar ()
		{
			GUILayout.BeginHorizontal (EditorStyles.toolbar);

			if (Search ()) {
				CreateVariableList ();
			}

			GUILayout.Space (2f);

			bool enabled = GUI.enabled;
			GUI.enabled = BehaviorSelection.activeBehavior != null;
			GUIStyle style = new GUIStyle("ToolbarCreateAddNewDropDown");
			GUIContent content = EditorGUIUtility.IconContent("CreateAddNew");

			if (GUILayout.Button (content, style, GUILayout.Width(35f))) {
				AddVariable ();
			}
			GUI.enabled = enabled;
			GUILayout.EndHorizontal ();
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
					BehaviorSelection.activeBehaviorTree.blackboard.Add (variable);
					CreateVariableList ();
                    BehaviorSelection.SetDirty();
                });

			}

			menu.ShowAsContext ();
		}

		private void CreateVariableList ()
		{
			if (BehaviorSelection.activeBehavior == null) {
				return;
			}
			Variable[] variables = BehaviorSelection.activeBehaviorTree.blackboard.GetAllVariables ();
			BehaviorSelection.activeBehaviorTree.blackboard.onChange -= CreateVariableList;
			BehaviorSelection.activeBehaviorTree.blackboard.onChange += CreateVariableList;

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
			searchedVariables.RemoveAll (x => x.name == "Self");
			int index = 0;
			if (this.m_VariableList != null) {
				index = this.m_VariableList.index;
			}
			this.m_VariableList = new ReorderableList (searchedVariables.ToArray (), typeof(Variable), true, false, false, false) {
				onReorderCallback = new ReorderableList.ReorderCallbackDelegate (this.OnReorder),
				drawElementCallback = new ReorderableList.ElementCallbackDelegate (this.OnDrawVariable),
				onSelectCallback = new ReorderableList.SelectCallbackDelegate (this.OnSelect),
				headerHeight = 0f,
				elementHeight = 24f,

			};
			this.m_VariableList.index = index;
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
						GUI.FocusControl ("");
						currentEvent.Use ();
					}
					break;
				case EventType.MouseUp:
					if (!rect1.Contains (Event.current.mousePosition) && Event.current.clickCount > 0 && index == this.m_VariableList.index && this.m_RenameIndex != -1) {
						this.m_RenameIndex = -1;
						GUI.FocusControl ("");
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

				EditorGUIUtility.labelWidth = 95f;
				DrawVariable (rect2, GUIContent.none, item);
				EditorGUIUtility.labelWidth = 0f;
				if (EditorGUI.EndChangeCheck ()) {
                    BehaviorSelection.SetDirty();
                }
			}
		}

		private void DrawVariable (Rect rect, GUIContent label, Variable item)
		{
			if (item is BoolVariable) {
				item.RawValue = EditorGUI.Toggle (rect, label, (bool)item.RawValue);
			} else if (item is StringVariable) {
				item.RawValue = EditorGUI.TextField (rect, label, (string)item.RawValue);
			} else if (item is FloatVariable) {
				item.RawValue = EditorGUI.FloatField (rect, label, (float)item.RawValue);
			} else if (item is IntVariable) {
				item.RawValue = EditorGUI.IntField (rect, label, (int)item.RawValue);
			} else if (item is GameObjectVariable) {
				item.RawValue = (GameObject)EditorGUI.ObjectField (rect, label, (GameObject)item.RawValue, typeof(GameObject), true);
			} else if (item is MaterialVariable) {
				item.RawValue = (Material)EditorGUI.ObjectField (rect, label, (Material)item.RawValue, typeof(Material), false);
			} else if (item is ColorVariable) {
				item.RawValue = EditorGUI.ColorField (rect, label, (Color)item.RawValue);
			} else if (item is ObjectVariable) {
				item.RawValue = (UnityEngine.Object)EditorGUI.ObjectField (rect, label, (UnityEngine.Object)item.RawValue, typeof(UnityEngine.Object), false);
			} else if (item is TransformVariable) {
				item.RawValue = (Transform)EditorGUI.ObjectField (rect, label, (Transform)item.RawValue, typeof(Transform), true);
			} else if (item is Vector2Variable) {
				item.RawValue = EditorGUI.Vector2Field (rect, label, (Vector2)item.RawValue);
			} else if (item is Vector3Variable) {
				item.RawValue = EditorGUI.Vector3Field (rect, label, (Vector3)item.RawValue);
			} else if (item is Vector4Variable) {
				item.RawValue = EditorGUI.Vector4Field (rect, label, (Vector4)item.RawValue);
			} else if (item is SpriteVariable) {
				item.RawValue = (Sprite)EditorGUI.ObjectField (rect, label, (Sprite)item.RawValue, typeof(UnityEngine.Sprite), false);
			} 
		}

		private void UpdateNodeVariables (string name, string newName)
		{
			Task[] nodes = BehaviorUtility.GetAllNodes (BehaviorSelection.activeBehaviorTree);
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

		private void OnSelect (ReorderableList list)
		{
			if (this.m_RenameIndex != list.index) {
				this.m_RenameIndex = -1;
			}
		}

		private void OnReorder (ReorderableList list)
		{
			BehaviorSelection.activeBehaviorTree.blackboard.Clear ();
			Variable[] variables = list.list.Cast<Variable> ().ToArray ();
			for (int i = 0; i < variables.Length; i++) {
				variables [i].index = i;
			}
			BehaviorSelection.activeBehaviorTree.blackboard.Add (new GameObjectVariable ("Self"){ isShared = true });
			BehaviorSelection.activeBehaviorTree.blackboard.AddRange (variables);
            BehaviorSelection.SetDirty();
        }

		private void DeleteVariable (int index)
		{
			Variable item = this.m_VariableList.list [index] as Variable;
			BehaviorSelection.activeBehaviorTree.blackboard.Remove (item);
			CreateVariableList ();
            BehaviorSelection.SetDirty();
        }

		private bool Search ()
		{
			bool changed;
			this.m_SearchString = SearchField (this.m_SearchString, out changed, GUILayout.MinWidth (155f));
			return changed;
		}

		private string SearchField (string search, out bool changed, params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginHorizontal ();
			changed = false;
			string before = search;

			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, "ToolbarSeachTextField", options);
			Rect buttonRect = rect;
			buttonRect.x = rect.width - 14;
			buttonRect.width = 14;

			if (!String.IsNullOrEmpty(before))
				EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Arrow);

			if (Event.current.type == EventType.MouseUp && buttonRect.Contains(Event.current.mousePosition) || before == "Search..." && GUI.GetNameOfFocusedControl() == "SearchTextFieldFocus") {
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
			string after = EditorGUI.TextField (rect,"", before, style);
		
			if (GUI.changed){
				EditorGUI.FocusTextInControl("SearchTextFieldFocus");
			}else if (string.IsNullOrEmpty(after) && GUI.GetNameOfFocusedControl() != "SearchTextFieldFocus") {
				after = before = "Search...";
			}

			GUI.Button(buttonRect, GUIContent.none,(after != "" && after != "Search...") ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");
			EditorGUILayout.EndHorizontal ();
			if (before != after){
				changed = true;
			}
			return after;
		}

	}
}