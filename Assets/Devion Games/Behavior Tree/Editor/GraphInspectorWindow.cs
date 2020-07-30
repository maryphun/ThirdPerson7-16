using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class GraphInspectorWindow : EditorWindow
	{
		private int m_SidebarItemIndex;
		private string[] m_SidebarItemContent = new string[]{ "Inspector", "Variables", "Preferences" };
		private VariableInspector m_VariableInspector;
		private GraphInspector<Task> m_GraphInspector;
		private SerializedObject serializedObject;
		private Vector2 m_SidebarScrollPosition;
		public BehaviorTreeGraph m_Graph;

		[MenuItem ("Tools/Devion Games/Behavior Trees/Graph Inspector", false, 2)]
		public static void ShowWindow ()
		{
			GraphInspectorWindow window = EditorWindow.GetWindow<GraphInspectorWindow> (false, "Graph Inspector");
			window.wantsMouseMove = false;
			window.minSize = new Vector2 (230f, 200f);
		}

		private void OnEnable ()
		{
			if (this.m_VariableInspector == null) {
				this.m_VariableInspector = new VariableInspector ();
			}

			if (this.m_GraphInspector == null) {
				this.m_GraphInspector = new GraphInspector<Task> ();
			}
		}

		private void OnGUI ()
		{
			if (BehaviorTreeWindow.current != null && this.m_Graph != BehaviorTreeWindow.current.m_Graph) {
				this.m_Graph = BehaviorTreeWindow.current.m_Graph;
				this.m_Graph.onSelectionChange += OnTaskSelectionChange;
			}
			EditorGUI.BeginChangeCheck ();
			DrawToolbar ();
			Draw ();
			if (EditorGUI.EndChangeCheck ()) {
				BehaviorUtility.Save (BehaviorSelection.activeBehaviorTree);
                BehaviorSelection.SetDirty();
				ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
			}
		}

		private void OnTaskSelectionChange (List<Task> selection)
		{
			Repaint ();
		}

		private void DrawToolbar ()
		{
			this.m_SidebarItemIndex = GUILayout.Toolbar (this.m_SidebarItemIndex, this.m_SidebarItemContent, EditorStyles.toolbarButton);
		}

		private void Draw ()
		{
			//GUILayout.BeginArea (position, Styles.sidebarBackground);
			this.m_SidebarScrollPosition = GUILayout.BeginScrollView (this.m_SidebarScrollPosition);
			switch (this.m_SidebarItemIndex) {
			case 0:
					if (m_Graph == null)
					{
						this.m_Graph = new BehaviorTreeGraph();
						this.m_Graph.onSelectionChange += OnTaskSelectionChange;
					}

					if (this.m_Graph.m_Selection.Count == 0)
					{
						if (BehaviorSelection.activeBehavior != null)
						{
							if (serializedObject == null || serializedObject.targetObject != (BehaviorSelection.activeBehavior as UnityEngine.Object))
							{
								serializedObject = new SerializedObject(BehaviorSelection.activeBehavior as UnityEngine.Object);
							}

							serializedObject.Update();

							EditorGUI.BeginChangeCheck();
							SerializedProperty script = serializedObject.FindProperty("m_Script");
							SerializedProperty behavior = serializedObject.FindProperty("m_BehaviorTree");
							SerializedProperty template = serializedObject.FindProperty("m_Template");
							bool enabled = GUI.enabled;
							GUI.enabled = false;
							EditorGUILayout.PropertyField(script);
							GUI.enabled = enabled;


							SerializedObject templateObject = null;
							if (template != null && template.objectReferenceValue != null)
							{
								templateObject = new SerializedObject(template.objectReferenceValue);
								behavior = templateObject.FindProperty("m_BehaviorTree");
								templateObject.Update();
							}

							EditorGUILayout.PropertyField(behavior.FindPropertyRelative("m_Name"));

							if (!(BehaviorSelection.activeBehavior is BehaviorTemplate))
							{

								Object cur = template.objectReferenceValue;
								EditorGUILayout.PropertyField(template);
								if (cur != template.objectReferenceValue)
								{
									m_Graph.m_Selection.Clear();
									m_Graph.CenterGraphView();
									Repaint();
								}
								EditorGUILayout.PropertyField(serializedObject.FindProperty("startWhenEnabled"));
								EditorGUILayout.PropertyField(serializedObject.FindProperty("restartWhenComplete"));
								EditorGUILayout.PropertyField(serializedObject.FindProperty("checkConfiguration"));
								EditorGUILayout.PropertyField(serializedObject.FindProperty("showSceneIcon"));
							}

							EditorGUILayout.PropertyField(behavior.FindPropertyRelative("description"));

							if (EditorGUI.EndChangeCheck())
							{
								serializedObject.ApplyModifiedProperties();
								if (templateObject != null)
								{
									templateObject.ApplyModifiedProperties();
								}
							}
						}
					}
					else
					{
						this.m_GraphInspector.OnGUI(this.m_Graph, position);
					}
					break;
			case 1:
				this.m_VariableInspector.OnGUI ();
				break;
			case 2:
				Preferences.OnGUI ();
				break;
			}

			GUILayout.EndScrollView ();
			//GUILayout.EndArea ();
		}
	}
}
