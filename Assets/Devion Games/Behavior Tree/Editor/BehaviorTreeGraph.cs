using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class BehaviorTreeGraph : Graph<Task>
	{
		private const int MIN_NODE_WIDTH = 120;
		private const int MIN_NODE_HEIGHT = 50;

		protected override Task[] inspectedNodes {
			get { 
				return BehaviorUtility.GetAllNodes (BehaviorSelection.activeBehaviorTree, false);	
			}
		}

		public override Task[] nodes {
			get { 
				return BehaviorUtility.GetAllNodes (BehaviorSelection.activeBehaviorTree, true);	
			}
		}

		private Task m_ConnectingStartNode;
		private int m_ConnectingIndex;
		private Vector2 m_ConnectingStartPosition;
		private string m_Copy;
		private Blackboard m_CopyBlackboard;
		private Dictionary<Type,GUIContent> taskContent;
  
		public BehaviorTreeGraph ()
		{
			Type[] taskTypes = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (assembly => assembly.GetTypes ()).Where (type => typeof(Task).IsAssignableFrom (type) && !type.IsAbstract && type != typeof(EntryTask)).ToArray ();
			taskTypes = taskTypes.OrderBy (x => x.BaseType.Name).ToArray ();
			taskContent = new Dictionary<Type, GUIContent> ();
			for (int i = 0; i < taskTypes.Length; i++) {
				Type type = taskTypes [i];

				string category = type.GetCategory ();
				if (!string.IsNullOrEmpty (category)) {
					category = category + "/";
				}
				GUIContent content = new GUIContent ("Add Task/" + type.BaseType.Name + "s/" + category + type.Name);
				taskContent.Add (type, content);
			}

			BehaviorSelection.selectionChanged += this.DeselectAll;
			BehaviorSelection.selectionChanged += this.CenterGraphView;
			#if UNITY_2017_2_OR_NEWER
			    EditorApplication.playModeStateChanged += delegate(PlayModeStateChange obj) {
			        this.CenterGraphView ();	
		    	};
            #else
			    EditorApplication.playmodeStateChanged += this.CenterGraphView;
            #endif
            Undo.undoRedoPerformed += delegate ()
            {
                ErrorChecker.CheckForErrors();
            };

        }

		protected override void OnGUI ()
		{
			this.InstructionGUI ();
			this.DescriptionGUI ();
		}

		private void InstructionGUI ()
		{
			GUILayout.BeginArea (new Rect (5f, 5f, 400f, this.m_GraphArea.height));

			if (BehaviorSelection.activeObject == null) {
				GUILayout.Label ("Select a GameObject or Template", Styles.notificationText);
			} else if (BehaviorSelection.activeBehaviorTree == null) {
				GUILayout.Label ("Right Click to add a Behavior Tree", Styles.notificationText);
			} else {
				GUILayout.Label (BehaviorSelection.activeObject.name + " - " + BehaviorSelection.activeBehaviorTree.name + (BehaviorSelection.activeBehavior.IsTemplate () ? "(Template)" : ""), Styles.notificationText);
				if (Preferences.GetBool (Preference.ShowBehaviorDescription)) {
					GUILayout.Label (BehaviorSelection.activeBehaviorTree.description, Styles.descriptionText);
				}
			}
			GUILayout.EndArea ();

		}

		private void DescriptionGUI ()
		{
			if (this.m_Selection.Count == 1 && Preferences.GetBool (Preference.ShowSelectedTaskDescription)) {
				Task node = this.m_Selection [0];
				GUI.Label (new Rect (5f, -3f, 350f, this.m_GraphArea.height), node.GetType ().GetTooltip (), Styles.descriptionText);
			}

		}

        protected override void InitializePreferences() {
            this.m_ConnectionStyle = Preferences.GetEnum<ConnectionStyle>(Preference.ConnectionStyle);
            this.m_MoveChildrenOnDrag = Preferences.GetBool(Preference.MoveChildrenOnDrag);
            this.m_ReorderChildrenByPosition = Preferences.GetBool(Preference.ReorderChildrenByPosition);
            this.m_OpenInspectorOnTaskClick = Preferences.GetBool(Preference.OpenInspectorOnTaskClick);
            this.m_OpenInspectorOnTaskDoubleClick = Preferences.GetBool(Preference.OpenInspectorOnTaskDoubleClick);
        }

        protected override void HandleKeyEvents(KeyCode key)
        {
			switch (key) {
				case KeyCode.Space:
					Vector2 mousePosition = Event.current.mousePosition / this.m_GraphZoom - this.m_GraphOffset;
					Vector2 pos = (mousePosition + this.m_GraphOffset) * this.m_GraphZoom + this.m_GraphArea.position;
					AddTaskWindow.ShowWindow(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y-21f, 230f, 21f), mousePosition);
					break;
				case KeyCode.Delete:
					ExecuteCommand("Delete");
					break;
			}
        }

        protected override void DrawNode (Rect rect, Task node, bool selected)
		{
			Color color = GUI.color;
			if (BehaviorUtility.SelfOrParentDisabled (node)) {
				GUI.color = new Color (1f, 1f, 1f, 0.6f);
			}
			GUIStyle style = Styles.skin.box;
			if (EditorApplication.isPlaying) {
				switch (node.status) {
				case TaskStatus.Running:
					style = Styles.skin.GetStyle ("Box Grey");
					break;
				case TaskStatus.Success:
					style = Styles.skin.GetStyle ("Box Green");
					break;
				case TaskStatus.Failure:
					style = Styles.skin.GetStyle ("Box Red");
					break;
				}
			}

			if (selected) {
				GUI.Box (rect, "", Styles.skin.GetStyle ("Box Selected"));
			}
			GUI.Box (rect, new GUIContent (node.name, Preferences.GetBool (Preference.ShowTaskCommentAsTooltip) ? node.nodeInfo.comment : ""), style);

			Texture2D icon = node.GetType ().GetIcon ();

		
			if (icon != null) {
				GUI.Label (new Rect (rect.center.x - 25f, rect.center.y - 5f, 50f, 25f), icon);
            }

			if (ErrorChecker.HasErrors (node)) {
				GUI.Label (new Rect (rect.x + rect.width - 13.5f, rect.y - 5f, 25, 25), Styles.errorIcon);
			}

			if (node.nodeInfo.isBreakpoint) {
				GUI.Label (new Rect (rect.x + rect.width - 25f, rect.y + rect.height - 25f, 25f, 25f), Styles.breakpoint);
			}
			GUI.color = color;
		}

		protected override void DrawNodeConnections (Task[] nodes, Vector2 offset)
		{
			if (Event.current.type != EventType.Repaint) {
				return;
			}
			for (int i = 0; i < nodes.Length; i++) {
				Task parent = nodes [i];
				if (parent.maxChildCount < 1) {
					continue;
				}
				Color color = GUI.color;
				bool active = !BehaviorUtility.SelfOrParentDisabled (parent);
				if (!active) {
					GUI.color = new Color (1f, 1f, 1f, 0.6f);
				}
				int count = parent.children.Count;
				float pinSize = 13f;
				float width = count * pinSize;
				width = Mathf.Clamp (width, pinSize, width);
				float extraWidth = 0f;
				if (parent.maxChildCount > count && count > 0) {
					extraWidth = pinSize * 2f;
				}
				Rect parentRect = GetNodeRect (parent, offset);
				Styles.pinBox.Draw (new Rect (parentRect.center.x - width * 0.5f - 6.5f - extraWidth * 0.5f, parentRect.y + parentRect.height - 5f, width + 13f + extraWidth, 10f), false, false, false, false);
				if (parent.maxChildCount > count) {
					if (count > 0) {
						DrawPin (new Rect (parentRect.center.x - width * 0.5f - pinSize, parentRect.center.y + parentRect.height * 0.5f - 3f, 13, 13f));
						DrawPin (new Rect (parentRect.center.x + width * 0.5f, parentRect.center.y + parentRect.height * 0.5f - 3f, 13, 13f));
					} else {
						DrawPin (new Rect (parentRect.center.x - 6.5f, parentRect.center.y + parentRect.height * 0.5f - 3f, 13, 13f));
					}
				}
				for (int c = 0; c < parent.children.Count; c++) {
					Rect pinRect = new Rect (parentRect.center.x + c * (pinSize) - width * 0.5f, parentRect.center.y + parentRect.height * 0.5f - 3f, 13, 13f);
					Styles.pinBackground.Draw (pinRect, false, false, false, false);
					Rect childRect = GetNodeRect (parent.children [c], offset);
					if (nodes.Contains (parent.children [c]) && (this.m_ConnectingStartNode != parent || this.m_ConnectingStartNode == parent && this.m_ConnectingIndex != c)) {
						DrawConnection (pinRect.center, childRect.position + new Vector2 (childRect.width * 0.5f, 1f), Preferences.GetEnum<ConnectionStyle> (Preference.ConnectionStyle), active ? new Color (0.506f, 0.62f, 0.702f, 1f) : new Color (0.506f, 0.62f, 0.702f, 0.5f));
					}
					Styles.pinActive.Draw (pinRect, false, false, false, false);
				}
				GUI.color = color;
			}
		}

		protected override void ReorderNodes (Task node)
		{
			Task parent = node.parent;
			if (node.parent != null) {
				List<Task> children = node.parent.children.OrderBy (x => x.nodeInfo.position.x).ToList ();
				node.parent.children.Clear ();
				for (int i = 0; i < children.Count; i++) {
					node.parent.children.Add (children [i]);
				}
			}
		}

		protected override void ConnectNodes (Task[] nodes, Vector2 offset)
		{
			Event currentEvent = Event.current;
			int controlID = GUIUtility.GetControlID (FocusType.Passive);

			switch (currentEvent.type) {
			case EventType.MouseDown:
				for (int i = 0; i < nodes.Length; i++) {
					Task node = nodes [i];

					Rect nodeRect = GetNodeRect (node, offset);
					if (node.maxChildCount > 0) {
						int count = node.children.Count;
						bool connect = false;

						float width = count * 13f;
						Rect rect = new Rect (nodeRect.center.x - width * 0.5f - 13f, nodeRect.center.y + nodeRect.height * 0.5f - 3f, 13, 13f);
						Rect rect1 = new Rect (nodeRect.center.x + width * 0.5f, nodeRect.center.y + nodeRect.height * 0.5f - 3f, 13, 13f);
						Rect rect2 = new Rect (nodeRect.center.x - 6.5f, nodeRect.center.y + nodeRect.height * 0.5f - 3f, 13, 13f);
						if (rect.Contains (currentEvent.mousePosition) && count > 0) {
							//insert -1
							this.m_ConnectingIndex = -1;
							this.m_ConnectingStartPosition = rect.center;
							connect = true;
						} else if (rect1.Contains (currentEvent.mousePosition) && count > 0) {
							//insert last
							this.m_ConnectingIndex = node.children.Count;
							this.m_ConnectingStartPosition = rect1.center;
							connect = true;
						} else if (rect2.Contains (currentEvent.mousePosition) && count == 0) {
							this.m_ConnectingIndex = 0;
							this.m_ConnectingStartPosition = rect2.center;
							connect = true;
						}

						for (int c = 0; c < count; c++) {
							Rect pinRect = new Rect (nodeRect.center.x + c * 13f - width * 0.5f, nodeRect.center.y + nodeRect.height * 0.5f - 3f, 13f, 13f);
							if (pinRect.Contains (currentEvent.mousePosition)) {
								//insert c
								this.m_ConnectingIndex = c;
								this.m_ConnectingStartPosition = pinRect.center;

								connect = true;
								break;
							}
						}

						if (connect) {
							GUIUtility.hotControl = controlID;
							m_ConnectingStartNode = node;
							currentEvent.Use ();
						}
					}
				}

				break;
			case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        Task node = null;
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            Rect nodeRect = GetNodeRect(nodes[i], offset);
                            if (nodeRect.Contains(currentEvent.mousePosition))
                            {
                                node = nodes[i];
                                break;
                            }
                        }

                        BehaviorSelection.RecordUndo("Connect");
                        if (node != null && this.m_ConnectingStartNode != node)
                        {
                            List<Task> children = new List<Task>();
                            BehaviorUtility.GetNodesRecursive(node, ref children);
                            if (!children.Contains(this.m_ConnectingStartNode))
                            {
                                if (this.m_ConnectingIndex >= 0 && this.m_ConnectingIndex < this.m_ConnectingStartNode.children.Count)
                                {
                                    //Remove Current Child
                                    BehaviorSelection.activeBehaviorTree.detachedNodes.Add(this.m_ConnectingStartNode.children[this.m_ConnectingIndex]);
                                    this.m_ConnectingStartNode.RemoveAt(this.m_ConnectingIndex);
                                }
                                if (node.parent != null)
                                {
                                    node.parent.Remove(node);
                                }
                                //Insert new child
                                this.m_ConnectingStartNode.Insert(Mathf.Clamp(this.m_ConnectingIndex, 0, this.m_ConnectingStartNode.children.Count), node);
                                BehaviorSelection.activeBehaviorTree.detachedNodes.Remove(node);
                            }
                        }
                        else
                        {

                            if (this.m_ConnectingIndex > -1 && this.m_ConnectingIndex < m_ConnectingStartNode.children.Count)
                            {
                                Task child = m_ConnectingStartNode.children[this.m_ConnectingIndex];
                                m_ConnectingStartNode.RemoveAt(this.m_ConnectingIndex);
                                BehaviorSelection.activeBehaviorTree.detachedNodes.Add(child);
                            }

                        }

						m_ConnectingStartNode = null;
                        GUIUtility.hotControl = 0;
                        ErrorChecker.CheckForErrors();
                        Event.current.Use();

                    }
                    break;
			}

			if (m_ConnectingStartNode != null) {
				DrawConnection (m_ConnectingStartPosition, currentEvent.mousePosition, Preferences.GetEnum<ConnectionStyle> (Preference.ConnectionStyle), new Color (0.506f, 0.62f, 0.702f, 1f));
				m_Host.Repaint ();
			}
		}

		protected override void MoveNode (Task node, Vector2 delta, bool moveChildren)
		{
			node.nodeInfo.position += delta;
			if (moveChildren) {
				for (int i = 0; i < node.children.Count; i++) {
					if (!this.m_Selection.Contains (node.children [i])) {
						MoveNode (node.children [i], delta, moveChildren);
					}
				}
			}

		}

		protected override Rect GetNodeRect (Task node, Vector2 offset)
		{
			int count = node.children.Count;
			float width = (count + 2) * 15f;
			Vector2 size = EditorStyles.label.CalcSize (new GUIContent (node.name));
			width = Mathf.Clamp (width, size.x + 15f, float.PositiveInfinity);
			Rect rect = new Rect (node.nodeInfo.position.x + offset.x, node.nodeInfo.position.y + offset.y, Mathf.Clamp (width, MIN_NODE_WIDTH, float.PositiveInfinity), MIN_NODE_HEIGHT);
			return rect;
		}

		protected override void GraphContextMenu (Vector2 position)
		{
            GenericMenu menu = new GenericMenu();
         
            if (BehaviorSelection.activeBehavior != null)
            {
                menu.AddItem(new GUIContent("Add Task"), false, delegate ()
                {
                    Vector2 pos = (position + this.m_GraphOffset) * this.m_GraphZoom + this.m_GraphArea.position;
                    AddTaskWindow.ShowWindow(new Rect(pos.x, pos.y, 230f, 21f), position);
                });

                if (!string.IsNullOrEmpty(this.m_Copy))
                {
                    menu.AddItem(new GUIContent("Paste Tasks"), false, delegate {
                        BehaviorSelection.RecordUndo("Paste");
                        this.PasteNodes(position);
                    });

                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Paste Tasks"));
                }
            } else {
                menu.AddDisabledItem(new GUIContent("Add Task"));
            }

		
			menu.AddSeparator ("");
			if (BehaviorSelection.activeBehavior != null) {
				menu.AddItem (new GUIContent ("Select All"), false, delegate {
					this.SelectAll ();
				});
				menu.AddItem (new GUIContent ("Invert Selection"), false, delegate {
					this.InvertSelection ();
				});
			} else {
				menu.AddDisabledItem (new GUIContent ("Select All"));
				menu.AddDisabledItem (new GUIContent ("Invert Selection"));
			}

            if (BehaviorSelection.activeGameObject != null)
            {
                menu.AddSeparator("");

                menu.AddItem(new GUIContent("Add Behavior"), false, delegate ()
                {
                    string[] names = BehaviorSelection.activeGameObject.GetComponents<Behavior>().Select(x => x.GetBehaviorTree().name).ToArray();
                    BehaviorSelection.activeBehavior = Undo.AddComponent<Behavior>(BehaviorSelection.activeGameObject);
                    EntryTask entryTask = new EntryTask();
                    entryTask.name = "Entry Task";
                    entryTask.nodeInfo.position = position;
                    BehaviorSelection.activeBehaviorTree.root = entryTask;

                    BehaviorSelection.activeBehaviorTree.name = ObjectNames.GetUniqueName(names, BehaviorSelection.activeBehaviorTree.name);

                    this.m_Selection.Clear();
                    this.CenterGraphView();
                    ErrorChecker.CheckForErrors();
                    this.m_Host.Repaint();
                });

                if (BehaviorSelection.activeBehavior != null)
                {
                    menu.AddItem(new GUIContent("Remove Behavior"), false, delegate ()
                    {
                        this.m_Selection.Clear();
                        ErrorChecker.ClearErrors();
                        Undo.DestroyObjectImmediate(BehaviorSelection.activeBehavior as UnityEngine.Object);
                        BehaviorSelection.activeBehavior = BehaviorSelection.activeGameObject.GetComponent<IBehavior>();
                        this.CenterGraphView();
                        this.m_Host.Repaint();
                    });

                    menu.AddItem(new GUIContent("Save as Template"), false, delegate ()
                    {
                        BehaviorTemplate template = AssetCreator.CreateAsset<BehaviorTemplate>(true);
                        if (template != null)
                        {
                            Selection.activeObject = template;
                            template.GetBehaviorTree().serializationData = BehaviorSelection.activeBehaviorTree.serializationData;
                            template.GetBehaviorTree().serializedObjects = BehaviorSelection.activeBehaviorTree.serializedObjects;
                            template.GetBehaviorTree().blackboard = new Blackboard(BehaviorSelection.activeBehaviorTree.blackboard);
                            BehaviorUtility.Load(template.GetBehaviorTree());
                            EditorUtility.SetDirty(template);
                        }

                    });
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Remove Behavior"));
                    menu.AddDisabledItem(new GUIContent("Save as Template"));
                }
            }
            else {
                menu.AddDisabledItem(new GUIContent("Add Behavior"));
                menu.AddDisabledItem(new GUIContent("Remove Behavior"));
                menu.AddDisabledItem(new GUIContent("Save as Template"));
            }
            menu.AddItem(new GUIContent("Create Template"), false, delegate () {
                BehaviorTemplate template = AssetCreator.CreateAsset<BehaviorTemplate>(true);
                if (template != null)
                {
                    Selection.activeObject = template;

                    EntryTask entryTask = new EntryTask();
                    entryTask.name = "Entry Task";
                    entryTask.nodeInfo.position = position;

                    BehaviorSelection.activeBehavior = template;
                    BehaviorSelection.activeBehaviorTree.root = entryTask;
                    this.m_Selection.Clear();
                    this.CenterGraphView();
                    ErrorChecker.CheckForErrors();
                    this.m_Host.Repaint();
                    EditorUtility.SetDirty(template);
                }

            });
            menu.ShowAsContext ();
		}

		protected override void NodeContextMenu (Task node, Vector2 position)
		{
			if (node != null && node.GetType () != typeof(EntryTask)) {
				GenericMenu menu = new GenericMenu ();
				this.m_Selection.RemoveAll (x => x.GetType () == typeof(EntryTask));
				string s = (this.m_Selection.Count > 1 ? "s" : "");
				menu.AddItem (new GUIContent ("Copy Task" + s), false, new GenericMenu.MenuFunction (this.CopyNodes));
				if (!string.IsNullOrEmpty (this.m_Copy)) {
					menu.AddItem (new GUIContent ("Paste Task" + s), false, delegate() {
						BehaviorSelection.RecordUndo ("Paste");
						this.PasteNodes (position);
					});
				} else {
					menu.AddDisabledItem (new GUIContent ("Paste Task" + s));
				}
				menu.AddItem (new GUIContent ("Cut Task" + s), false, delegate {
					SaveSelection ();
					BehaviorSelection.RecordUndo ("Cut");
					this.CutNodes ();
				});
				menu.AddItem (new GUIContent ("Delete Task" + s), false, delegate {
					SaveSelection ();
					BehaviorSelection.RecordUndo ("Delete");
					this.DeleteNodes ();
				});

				if (this.m_Selection.Count == 1) {
					menu.AddSeparator ("");
					menu.AddItem (new GUIContent (node.enabled ? "Disable" : "Enable"), false, delegate() {
						BehaviorSelection.RecordUndo (node.enabled ? "Disable" : "Enable");
						node.enabled = !node.enabled;
						ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
					});

					if (node.maxChildCount > 0) {
						menu.AddItem (new GUIContent ("Collapse"), node.nodeInfo.isCollapsed, delegate() {
							BehaviorSelection.RecordUndo ("Collapse");
							node.nodeInfo.isCollapsed = !node.nodeInfo.isCollapsed;
							if (node.nodeInfo.isCollapsed) {
								List<Task> nodes = new List<Task> ();
								BehaviorUtility.GetNodesRecursive (node, ref nodes, true);
								nodes.Remove (node);
								this.m_Selection.RemoveAll (x => nodes.Contains (x));
							}
						});
					}
					menu.AddItem (new GUIContent ("Breakpoint"), node.nodeInfo.isBreakpoint, delegate() {
						BehaviorSelection.RecordUndo ("Breakpoint");
						node.nodeInfo.isBreakpoint = !node.nodeInfo.isBreakpoint;
						if (EditorApplication.isPaused) {
							EditorApplication.isPaused = false;
						}

					});
				}
				menu.ShowAsContext ();
			}
		}

		protected override void ExecuteCommand (string name)
		{
			switch (name) {
			case "Copy":
				this.CopyNodes ();
				break;
			case "Paste":
				BehaviorSelection.RecordUndo ("Paste");
				this.PasteNodes (Vector2.zero);
				break;
			case "Cut":
				SaveSelection ();
				BehaviorSelection.RecordUndo ("Cut");
				this.CutNodes ();
				break;
			case "Duplicate":
				BehaviorSelection.RecordUndo ("Duplicate");
				this.DuplicateNodes ();
				break;
			case "Delete":
				SaveSelection ();
				BehaviorSelection.RecordUndo ("Delete");
				this.DeleteNodes ();
				break;
			case "SelectAll":
				this.SelectAll ();
				break;
			case "DeselectAll":
				this.DeselectAll ();
				break;
			case "CenterGraph":
				this.CenterGraphView ();
				break;
			}
		}

		private void CopyNodes ()
		{
			BehaviorTree copy = new BehaviorTree ();
			copy.serializationData = BehaviorSelection.activeBehaviorTree.serializationData;

			BehaviorUtility.Load (copy);
			Task[] toDelete = BehaviorUtility.GetAllNodes (copy).Where (x => !this.m_Selection.Exists (y => x.nodeInfo.id == y.nodeInfo.id)).ToArray (); 
			BehaviorUtility.DeleteNodes (copy, toDelete);
			this.m_Copy = BehaviorUtility.Save (copy);
			m_CopyBlackboard = new Blackboard (BehaviorUtility.GetReferencedVariables (BehaviorSelection.activeBehaviorTree.blackboard, BehaviorUtility.GetAllNodes (copy)));
		}

		private void PasteNodes (Vector2 position)
		{
			if (!string.IsNullOrEmpty (this.m_Copy)) {
				this.m_Selection.Clear ();
				BehaviorTree behaviorTree = new BehaviorTree ();
				behaviorTree.serializationData = this.m_Copy;

				Variable[] variables = BehaviorSelection.activeBehaviorTree.blackboard.GetAllVariables();
				Variable[] copyVariables = m_CopyBlackboard.GetAllVariables();
				List<Variable> pasteVariables = new List<Variable>();

				for (int i = 0; i < copyVariables.Length; i++)
				{
					if (!variables.Any(x => x.name == copyVariables[i].name) && !pasteVariables.Any(x => x.name == copyVariables[i].name))
					{
						pasteVariables.Add(copyVariables[i]);

					}

				}
				BehaviorSelection.activeBehaviorTree.blackboard.AddRange(pasteVariables.ToArray());

				BehaviorUtility.Load (behaviorTree);
				
				for (int i = 0; i < behaviorTree.detachedNodes.Count; i++) {
					Task node = behaviorTree.detachedNodes [i];
					if (position == Vector2.zero) {
						position = node.nodeInfo.position + new Vector2 (20, 15);
					}
					BehaviorUtility.OffsetNode (node, position - node.nodeInfo.position);
					BehaviorSelection.activeBehaviorTree.detachedNodes.Add (node);
				}
				behaviorTree.root = null;
				this.m_Selection.AddRange (BehaviorUtility.GetAllNodes (behaviorTree));
				ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
			}
		}

		private void DeleteNodes ()
		{
			this.m_Selection.Remove (BehaviorSelection.activeBehaviorTree.root);
			BehaviorUtility.DeleteNodes (BehaviorSelection.activeBehaviorTree, this.m_Selection.ToArray ());
			this.m_Selection.Clear ();
			BehaviorUtility.Save (BehaviorSelection.activeBehaviorTree);
			ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
		}

		private void CutNodes ()
		{
			CopyNodes ();
			DeleteNodes ();
			ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
		}

		private void DuplicateNodes ()
		{
			CopyNodes ();
			PasteNodes (Vector2.zero);
			ErrorChecker.CheckForErrors (BehaviorSelection.activeBehavior);
		}

		private void DrawPin (Rect rect)
		{
			Styles.pinBackground.Draw (rect, false, false, false, false);
			Styles.pinInactive.Draw (rect, false, false, false, false);
		}

		public void AlignNodesHorizontal ()
		{
			List<Task> nodes = this.m_Selection;
			Vector2 axisCenter = Vector2.zero;
			for (int i = 0; i < nodes.Count; i++) {
				Task node = nodes [i];
				axisCenter += GetNodeRect (node, Vector2.zero).position;
			}	
			axisCenter /= nodes.Count;
			for (int i = 0; i < nodes.Count; i++) {
				Task node = nodes [i];
				node.nodeInfo.position = new Vector2 (node.nodeInfo.position.x, axisCenter.y);
			}
		}

		public void AlignNodesVertical ()
		{
			List<Task> nodes = this.m_Selection;
			Vector2 axisCenter = Vector2.zero;
			for (int i = 0; i < nodes.Count; i++) {
				Task node = nodes [i];
				axisCenter += GetNodeRect (node, Vector2.zero).position;
			}	
			axisCenter /= nodes.Count;
			for (int i = 0; i < nodes.Count; i++) {
				Task node = nodes [i];
				node.nodeInfo.position = new Vector2 (axisCenter.x, node.nodeInfo.position.y);
			}
		}

		/*	public void AlignNodesTopCorner ()
		{
			List<Node> nodes = NodeSelection.selectedNodes;
			float bottom = float.PositiveInfinity;
			for (int i = 0; i < nodes.Count; i++) {
				Node node = nodes [i];
				float current = NodeDesignerUtility.GetNodeRect (node, Vector2.zero).y;
				if (current < bottom) {
					bottom = current;
				}
			}	
			for (int i = 0; i < nodes.Count; i++) {
				Node node = nodes [i];
				node.position = new Vector2 (node.position.x, bottom);
			}
		}

		public void AlignNodesBottomCorner ()
		{
			List<Node> nodes = NodeSelection.selectedNodes;
			float top = float.NegativeInfinity;
			for (int i = 0; i < nodes.Count; i++) {
				Node node = nodes [i];
				float current = NodeDesignerUtility.GetNodeRect (node, Vector2.zero).y;
				if (current > top) {
					top = current;
				}
			}	
			for (int i = 0; i < nodes.Count; i++) {
				Node node = nodes [i];
				node.position = new Vector2 (node.position.x, top);
			}
		}*/
	}
}
