using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
    public class AddTaskWindow : EditorWindow
    {
        private static AddTaskWindow.Styles m_Styles;
        private string m_SearchString=string.Empty;
        private Vector2 m_ScrollPosition;
        private Vector2 m_MousePosition;
        private TaskElement rootElement;
        private TaskElement selectedElement;
        private bool isSearching {
            get {
                return !string.IsNullOrEmpty(m_SearchString);
            }
        }


        public static void ShowWindow(Rect buttonRect, Vector2 mousePosition)
        {
            AddTaskWindow window = ScriptableObject.CreateInstance<AddTaskWindow>();
            window.m_MousePosition = mousePosition;
            buttonRect = GUIToScreenRect(buttonRect);
            window.ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, 320f));
        }

        private void OnEnable()
        {
            rootElement = BuildTaskElements();
            selectedElement = rootElement;
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (AddTaskWindow.m_Styles == null){
                AddTaskWindow.m_Styles = new AddTaskWindow.Styles();
            }
            if (rootElement == null){
                rootElement = BuildTaskElements();
                selectedElement = rootElement;
            }
            GUI.Label(new Rect(0f, 0f, base.position.width, base.position.height), GUIContent.none, AddTaskWindow.m_Styles.background);
            GUILayout.Space(5f);
    
            this.m_SearchString = SearchField(this.m_SearchString);
          
            GUIContent header = new GUIContent(!isSearching?selectedElement.label.text:"Search");
            Rect headerRect = GUILayoutUtility.GetRect(header, AddTaskWindow.m_Styles.header);

            if (GUI.Button(headerRect, header, AddTaskWindow.m_Styles.header))
            {
                if (selectedElement.parent != null && !isSearching){
                    selectedElement = selectedElement.parent;
                }
            }
            if (selectedElement.parent != null && !isSearching)
            {
                GUI.Label(new Rect(headerRect.x, headerRect.y + 4f, 16f, 16f), "", AddTaskWindow.m_Styles.leftArrow);
            }
            GUILayout.Space(-5);
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            if (isSearching) {
                TaskElement[] elements = GetAllElements(rootElement);
                DrawElements(elements);
            } else {
                DrawElements(selectedElement.children.ToArray());
            }
            GUILayout.EndScrollView();
        }

        private void DrawElements(TaskElement[] elements) {
            string[] searchArray = m_SearchString.ToLower().Split(' ');

            foreach (TaskElement element in elements)
            {
     
                string fullPath = element.path.ToLower()+ "." + element.label.text.ToLower();
   
                if (isSearching && (m_SearchString.Length <= 1 || !searchArray.All(fullPath.Contains) || element.children.Count>0))
                {
                    continue;
                }

                Color backgroundColor = GUI.backgroundColor;
                Color textColor = AddTaskWindow.m_Styles.elementButton.normal.textColor;

                Rect rect = GUILayoutUtility.GetRect(element.label, AddTaskWindow.m_Styles.elementButton, GUILayout.Height(20f));
                GUI.backgroundColor = (rect.Contains(Event.current.mousePosition) ? GUI.backgroundColor : new Color(0, 0, 0, 0.0f));
                AddTaskWindow.m_Styles.elementButton.normal.textColor = (rect.Contains(Event.current.mousePosition) ? Color.white : AddTaskWindow.m_Styles.elementButton.normal.textColor);
                GUIContent label = new GUIContent(element.label);
                if (element.type != null) {
                   label.text = isSearching ? element.type.GetCategory().Split('/').Last() + "." + element.label.text : element.label.text;
                   label.tooltip = element.type.GetTooltip();
                    
                }

                if (GUI.Button(rect, label, AddTaskWindow.m_Styles.elementButton))
                {
                    selectedElement = element;
                    if (selectedElement.children.Count == 0) {
                        BehaviorSelection.RecordUndo("Add");
                        if (BehaviorSelection.activeBehavior == null)
                        {
                            BehaviorSelection.activeBehavior = BehaviorSelection.activeGameObject.AddComponent<Behavior>();
                            BehaviorTreeWindow.current.m_Graph.m_Selection.Clear();
                        }
                        if (BehaviorSelection.activeBehaviorTree.root == null)
                        {
                            EntryTask entryTask = new EntryTask();
                            entryTask.name = "Entry Task";
                            entryTask.nodeInfo.position = new Vector2(position.x, position.y - 100f);
                            BehaviorSelection.activeBehaviorTree.root = entryTask;
                            BehaviorUtility.AddNode(BehaviorSelection.activeBehaviorTree, entryTask, element.type, m_MousePosition);
                        }
                        else
                        {
                            BehaviorUtility.AddNode(BehaviorSelection.activeBehaviorTree, null, element.type, m_MousePosition);
                        }
                        BehaviorUtility.Save(BehaviorSelection.activeBehaviorTree);
                        BehaviorSelection.SetDirty();
                        ErrorChecker.CheckForErrors();
                        Close();
                    }
                }
                GUI.backgroundColor = backgroundColor;
                AddTaskWindow.m_Styles.elementButton.normal.textColor = textColor;
                if (element.type != null)
                {

                    string category = element.type.GetCategory().Split('/').Last();
                    Texture2D icon = (Texture2D)EditorGUIUtility.ObjectContent(null, TypeUtility.GetType(category)).image;


                    if (icon != null)
                    {
                        GUI.Label(new Rect(rect.x, rect.y, 20f, 20f), icon);
                    }
                }
                if (element.children.Count > 0)
                {
                    GUI.Label(new Rect(rect.x + rect.width - 16f, rect.y + 2f, 16f, 16f), "", AddTaskWindow.m_Styles.rightArrow);
                }
            }
        }

        private TaskElement BuildTaskElements()
        {
            TaskElement root = new TaskElement("Task", "");
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(Task).IsAssignableFrom(type) && !type.IsAbstract && type != typeof(EntryTask)).ToArray();
            types = types.OrderBy(x => x.BaseType.Name).ToArray();
            foreach (Type type in types)
            {
                string baseType = type.BaseType.Name + "s";
                string category = type.GetCategory();
                string menu = baseType + (string.IsNullOrEmpty(category) ? "" : "/" + category);
                menu = menu.Replace("/", ".");

                string[] s = menu.Split('.');
                TaskElement prev = null;
                string cur = string.Empty;
                for (int i = 0; i < s.Length; i++)
                {
                    cur += (string.IsNullOrEmpty(cur) ? "" : ".") + s[i];
                    TaskElement parent = root.Find(cur);
                    if (parent == null)
                    {
                        parent = new TaskElement(s[i], cur);
                        if (prev != null)
                        {
                            parent.parent = prev;
                            prev.children.Add(parent);
                        }
                        else
                        {
                            parent.parent = root;
                            root.children.Add(parent);
                        }
                    }

                    prev = parent;

                }
                TaskElement element = new TaskElement(type.Name, menu);
                element.type = type;
                element.parent = prev;
                prev.children.Add(element);
            }
            return root;
        }

        private TaskElement[] GetAllElements(TaskElement root)
        {
            List<TaskElement> elements = new List<TaskElement>();
            GetTaskElements(root, ref elements);
            return elements.ToArray();
        }

        private void GetTaskElements(TaskElement current, ref List<TaskElement> list)
        {
            list.Add(current);
            for (int i = 0; i < current.children.Count; i++)
            {
                GetTaskElements(current.children[i], ref list);
            }
        }


       /* private string SearchField(string search, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            string before = search;
            GUI.SetNextControlName("AddTaskWindowSearch");
            string after = EditorGUILayout.TextField("", before, "SearchTextField", options);

            if (GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f))){
                after = string.Empty;
                GUIUtility.keyboardControl = 0;
            }
            else {
                GUI.FocusControl("AddTaskWindowSearch");
            }
            GUILayout.EndHorizontal();
            return after;
        }*/

        private string SearchField(string search, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();
            string before = search;

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, "ToolbarSeachTextField", options);
            rect.x += 2f;
            rect.width -= 2f;
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
            EditorGUI.FocusTextInControl("SearchTextFieldFocus");
           
            GUI.Button(buttonRect, GUIContent.none, (after != "" && after != "Search...") ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");
            EditorGUILayout.EndHorizontal();
            return after;
        }

        private static Rect GUIToScreenRect(Rect guiRect)
        {
            Vector2 vector = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
            guiRect.x = vector.x;
            guiRect.y = vector.y;
            return guiRect;
        }

        public class TaskElement
        {

            public Type type;
            public TaskElement parent;

            private string m_Path;

            public string path
            {
                get
                {
                    return this.m_Path;
                }
            }

            private GUIContent m_Label;

            public GUIContent label
            {
                get
                {
                    return this.m_Label;
                }
                set
                {
                    this.m_Label = value;
                }
            }

            public TaskElement(string label, string path)
            {
                this.label = new GUIContent(label);
                this.m_Path = path;
            }


            private List<TaskElement> m_children;

            public List<TaskElement> children
            {
                get
                {
                    if (this.m_children == null)
                    {
                        this.m_children = new List<TaskElement>();
                    }
                    return m_children;
                }
                set
                {
                    this.m_children = value;
                }
            }

            public bool Contains(TaskElement item)
            {
                if (item.label.text == label.text)
                {
                    return true;
                }
                for (int i = 0; i < children.Count; i++)
                {
                    bool contains = children[i].Contains(item);
                    if (contains)
                    {
                        return true;
                    }
                }
                return false;
            }

            public TaskElement Find(string path)
            {
                if (this.path == path)
                {
                    return this;
                }
                for (int i = 0; i < children.Count; i++)
                {
                    TaskElement tree = children[i].Find(path);
                    if (tree != null)
                    {
                        return tree;
                    }
                }
                return null;
            }
        }

        private class Styles
        {
            public GUIStyle header = new GUIStyle("DD HeaderStyle");
            public GUIStyle rightArrow = "AC RightArrow";
            public GUIStyle leftArrow = "AC LeftArrow";
            public GUIStyle elementButton = new GUIStyle("MeTransitionSelectHead");
            public GUIStyle background = "grey_border";

            public Styles()
            {

                this.header.stretchWidth = true;
                this.header.margin= new RectOffset(1,1,0,4);
               
                this.elementButton.alignment = TextAnchor.MiddleLeft;
                this.elementButton.padding.left = 22;
                this.elementButton.margin=new RectOffset(1,1,0,0);
            }
        }
    }
}