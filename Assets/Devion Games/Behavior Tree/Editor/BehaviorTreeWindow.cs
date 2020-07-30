using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class BehaviorTreeWindow : EditorWindow
	{
		public static BehaviorTreeWindow current;

		public GraphInspectorWindow graphInspectorWindow {
			get { 
				GraphInspectorWindow[] windows = Resources.FindObjectsOfTypeAll<GraphInspectorWindow> ();
				if (windows != null && windows.Length > 0) {
					return windows [0];
				}
				return null;
			}
		}

		private const  float SIDEBAR_MIN_WIDTH = 260f;
		private const float SIDEBAR_RESIZE_WIDTH = 10f;

		private Toolbar m_SidebarToolbar;
		private Rect m_SidebarPosition;
		private Vector2 m_SidebarScrollPosition;
		private Rect m_SidebarToolbarPosition;
		private bool m_SidebarVisibility = true;
		private bool m_ResizeSidebar;
		private int m_SidebarItemIndex;
		private string[] m_SidebarItemContent = new string[]{ "Inspector", "Variables", "Preferences" };

		public BehaviorTreeGraph m_Graph;
		private Toolbar m_GraphToolbar;
		private Rect m_GraphToolbarPosition;
		private Rect m_GraphPosition;
		private bool m_VisibilityStatus;
		private Rect m_BottomToolbarPosition;
		private Toolbar m_BottomToolbar;

		private VariableInspector m_VariableInspector;
		private GraphInspector<Task> m_GraphInspector;
		private string[] commandNames = new string[]{ "SelectInspector", "OnStartDrag", "OnEndDrag" };

		[MenuItem ("Tools/Devion Games/Behavior Trees/Editor", false, 0)]
		public static void ShowWindow ()
		{
			BehaviorTreeWindow window = EditorWindow.GetWindow<BehaviorTreeWindow> (false, "Behavior Tree");
			window.wantsMouseMove = false;
			window.minSize = new Vector2 (500f, 100f);
		}


		private void OnEnable ()
		{
			BehaviorTreeWindow.current = this;
			this.m_SidebarVisibility = EditorPrefs.GetBool ("BehaviorTreeWindow.m_SidebarVisibility", true);
			#if UNITY_2017_2_OR_NEWER
			EditorApplication.playModeStateChanged += OnPlaymodeStateChange;
			#else
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChange;
			#endif
			Undo.undoRedoPerformed += Repaint;

			if (this.m_Graph == null) {
				this.m_Graph = new BehaviorTreeGraph ();
				this.m_Graph.CenterGraphView ();
			}

			this.m_GraphToolbar = new Toolbar ();
			this.m_GraphToolbar.Add (new SelectObjectField ());
			this.m_GraphToolbar.Add (new SelectBehaviorField ());
			this.m_GraphToolbar.Add (new ActionField (this.m_Graph.ZoomField));
			this.m_GraphToolbar.Add (new LockSelectionField ());
			this.m_GraphToolbar.Add (new CenterGraphView ());
			this.m_GraphToolbar.Add (new AlignVerticalField ());
			this.m_GraphToolbar.Add (new AlignHorizontalField ());

			this.m_SidebarToolbar = new Toolbar ();
			this.m_SidebarToolbar.itemAlignment = ToolbarItemPosition.None;
			this.m_SidebarToolbar.Add (new ActionField (this.DrawSidebarToolbar));

			this.m_BottomToolbar = new Toolbar ();
			this.m_BottomToolbar.Add (new PlayField ());
			this.m_BottomToolbar.Add (new PauseField ());
			this.m_BottomToolbar.Add (new StepField ());
			this.m_BottomToolbar.Add (new ErrorField ());
			if (this.m_VariableInspector == null) {
				this.m_VariableInspector = new VariableInspector ();
			}

			if (this.m_GraphInspector == null) {
				this.m_GraphInspector = new GraphInspector<Task>();
			}
			if (graphInspectorWindow != null) {
				graphInspectorWindow.Repaint ();
			}
		}

		private bool m_DraggingNodes;

		private void OnGUI ()
		{
			SetupSizes ();
			ResizeSidebar ();
			EditorGUI.BeginChangeCheck ();
			this.m_SidebarToolbar.OnGUI (this.m_SidebarToolbarPosition);
			this.DrawSidebar ();
			this.m_GraphToolbar.OnGUI (this.m_GraphToolbarPosition);
			this.m_BottomToolbar.OnGUI (this.m_BottomToolbarPosition);
			if (EditorGUI.EndChangeCheck ()) {
				BehaviorUtility.Save (BehaviorSelection.activeBehaviorTree);
				ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
			}

			this.m_Graph.OnGUI (this, this.m_GraphPosition);
			Event currentEvent = Event.current;
			if (currentEvent.type == EventType.ValidateCommand && commandNames.Contains (currentEvent.commandName)) {
				currentEvent.Use (); 
			}
			if (currentEvent.type == EventType.ExecuteCommand) {
				string command = currentEvent.commandName;
				switch (command) {
				case "SelectInspector":
					this.m_SidebarItemIndex = 0;
					break;
				case "OnStartDrag":
					m_DraggingNodes = true;
                    BehaviorSelection.RecordCompleteUndo("Drag");
                    if (Preferences.GetBool (Preference.HideSidebarOnDrag)) {
						this.m_VisibilityStatus = this.m_SidebarVisibility;
						if (this.m_SidebarVisibility) {
							this.m_SidebarVisibility = false;
							this.m_Graph.m_GraphOffset.x += this.m_SidebarPosition.width / this.m_Graph.m_GraphZoom;
						}
					}
					break;
				case "OnEndDrag":
					BehaviorUtility.Save (BehaviorSelection.activeBehaviorTree);
                    BehaviorSelection.SetDirty();
					m_DraggingNodes = false;
					if (Preferences.GetBool (Preference.HideSidebarOnDrag)) {
						if (this.m_VisibilityStatus) {
							this.m_SidebarVisibility = true;
							this.m_Graph.m_GraphOffset.x -= this.m_SidebarPosition.width / this.m_Graph.m_GraphZoom;
						}
					}
					break;
				}
			}
		}

		private void SetupSizes ()
		{
			this.m_SidebarPosition = new Rect (0f, 21f, Mathf.Clamp (this.m_SidebarPosition.width, SIDEBAR_MIN_WIDTH, Screen.width - SIDEBAR_MIN_WIDTH), Screen.height - 18f - 21f);
			this.m_SidebarToolbarPosition = new Rect (0f, 0f, (this.m_SidebarVisibility ? this.m_SidebarPosition.width : 25f),21f);
			this.m_GraphPosition = new Rect ((this.m_SidebarVisibility ? this.m_SidebarPosition.width : 0f), 21f, Screen.width - (this.m_SidebarVisibility ? this.m_SidebarPosition.width : 0f), Screen.height - 18f - 21f - 21f);
			this.m_GraphToolbarPosition = new Rect ((this.m_SidebarVisibility ? this.m_SidebarPosition.width - 1f : 25f), 0f, this.m_GraphPosition.width, 21f);
			this.m_BottomToolbarPosition = new Rect (this.m_GraphPosition.x, this.m_GraphPosition.height + 21f, this.m_GraphPosition.width, 21f);
		}

		private void ResizeSidebar ()
		{
			Rect rect = new Rect (this.m_SidebarPosition.width - SIDEBAR_RESIZE_WIDTH * 0.5f, this.m_SidebarPosition.y, SIDEBAR_RESIZE_WIDTH, this.m_SidebarPosition.height);
			EditorGUIUtility.AddCursorRect (rect, MouseCursor.ResizeHorizontal);

			Event currentEvent = Event.current;
			switch (currentEvent.rawType) {
			case EventType.MouseDown:
				if (rect.Contains (currentEvent.mousePosition)) {
					this.m_ResizeSidebar = true;
					currentEvent.Use ();
				}
				break;
			case EventType.MouseUp:
				if (this.m_ResizeSidebar) {
					this.m_ResizeSidebar = false;
					currentEvent.Use ();
				}
				break;
			case EventType.MouseDrag:
				if (this.m_ResizeSidebar) {
					this.m_SidebarPosition.width = currentEvent.mousePosition.x;
					this.m_SidebarPosition.width = Mathf.Clamp (this.m_SidebarPosition.width, SIDEBAR_MIN_WIDTH, this.position.width - SIDEBAR_MIN_WIDTH);
					currentEvent.Use ();
				}
				break;
			}
		}

		#if UNITY_2017_2_OR_NEWER
		private void OnPlaymodeStateChange (PlayModeStateChange state)	

#else
		private void OnPlaymodeStateChange ()
		#endif
		{
			ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
			BehaviorUtility.Save (BehaviorSelection.activeBehaviorTree);
			Repaint ();
		}

		private void OnFocus ()
		{
			Repaint ();
		}

		private void OnSelectionChange ()
		{
			BehaviorUtility.Save (BehaviorSelection.activeBehaviorTree);
			Repaint ();
		}

		private void OnInspectorUpdate ()
		{
			if (EditorApplication.isPlaying) {
				Repaint ();
			}
		}

		private void DrawSidebarToolbar ()
		{
			if (this.m_SidebarVisibility) {
				this.m_SidebarItemIndex = GUILayout.Toolbar (this.m_SidebarItemIndex, this.m_SidebarItemContent, EditorStyles.toolbarButton);
				GUILayout.FlexibleSpace ();
			}
			if (GUILayout.Button (this.m_SidebarVisibility ? Styles.visibilityOn : Styles.visibilityOff, "label")) {
				this.m_SidebarVisibility = !this.m_SidebarVisibility;
				EditorPrefs.SetBool ("BehaviorTreeWindow.m_SidebarVisibility", this.m_SidebarVisibility);
			}
		}

		private SerializedObject serializedObject;

		private void DrawSidebar ()
		{
			if (!this.m_SidebarVisibility) {
				return;
			}
			GUILayout.BeginArea (this.m_SidebarPosition, Styles.sidebarBackground);
			this.m_SidebarScrollPosition = GUILayout.BeginScrollView (this.m_SidebarScrollPosition);
			switch (this.m_SidebarItemIndex) {
			case 0:
				if (m_DraggingNodes && !Preferences.GetBool (Preference.DrawInspectorOnDrag))
					break;
				
				if (this.m_Graph.m_Selection.Count == 0) {
					if (BehaviorSelection.activeBehavior != null) {
						if (serializedObject == null || serializedObject.targetObject != (BehaviorSelection.activeBehavior as UnityEngine.Object)) {
							serializedObject = new SerializedObject (BehaviorSelection.activeBehavior as UnityEngine.Object);
						}
						if (!m_DraggingNodes) {
							serializedObject.Update ();
						}
						EditorGUI.BeginChangeCheck ();
						SerializedProperty script = serializedObject.FindProperty ("m_Script");
						SerializedProperty behavior = serializedObject.FindProperty ("m_BehaviorTree");
						SerializedProperty template = serializedObject.FindProperty ("m_Template");
						bool enabled = GUI.enabled;
						GUI.enabled = false;
						EditorGUILayout.PropertyField (script);
						GUI.enabled = enabled;

					
						SerializedObject templateObject = null;
						if (template != null && template.objectReferenceValue != null) {
							templateObject = new SerializedObject (template.objectReferenceValue);
							behavior = templateObject.FindProperty ("m_BehaviorTree");
							templateObject.Update ();
						}
			
						EditorGUILayout.PropertyField (behavior.FindPropertyRelative ("m_Name"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("m_TickEvent"));

						if (!(BehaviorSelection.activeBehavior is BehaviorTemplate)) {

							Object cur = template.objectReferenceValue;
							EditorGUILayout.PropertyField (template);
							if (cur != template.objectReferenceValue) {
								m_Graph.m_Selection.Clear ();
								m_Graph.CenterGraphView ();
								Repaint ();
							}
							EditorGUILayout.PropertyField (serializedObject.FindProperty ("startWhenEnabled"));
							EditorGUILayout.PropertyField (serializedObject.FindProperty ("restartWhenComplete"));
							EditorGUILayout.PropertyField (serializedObject.FindProperty ("checkConfiguration"));
							EditorGUILayout.PropertyField (serializedObject.FindProperty ("showSceneIcon"));
						} 
		
						EditorGUILayout.PropertyField (behavior.FindPropertyRelative ("description"));

						if (EditorGUI.EndChangeCheck () && !m_DraggingNodes) {
							serializedObject.ApplyModifiedProperties ();
							if (templateObject != null) {
								templateObject.ApplyModifiedProperties ();
							}
						}
					}
				} else {
					
					this.m_GraphInspector.OnGUI (this.m_Graph, this.m_SidebarPosition);
				}
				break;
			case 1:
				if (m_DraggingNodes && !Preferences.GetBool (Preference.DrawInspectorOnDrag))
					break;
				this.m_VariableInspector.OnGUI ();
				break;
			case 2:
				if (m_DraggingNodes && !Preferences.GetBool (Preference.DrawInspectorOnDrag))
					break;
				Preferences.OnGUI ();
				break;
			}

			GUILayout.EndScrollView ();
			GUILayout.EndArea ();
		}
	}
}