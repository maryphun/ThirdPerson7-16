using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace DevionGames.BehaviorTrees
{
	public class TaskGenerator : EditorWindow
	{
		[MenuItem ("Tools/Devion Games/Behavior Trees/Task Generator", false, 6)]
		public static void ShowWindow ()
		{
			TaskGenerator window = EditorWindow.GetWindow<TaskGenerator> ("Node Generator");
			Vector2 size = new Vector2 (250f, 250f);
			window.minSize = size;
		}

		private Type[] types;

		private string selectedTypeString = string.Empty;
		private string selectedMemberInfoString = string.Empty;

		private List<string> outputText = new List<string> ();
		private List<string> outputNames = new List<string> ();
		private List<bool> outputSelection = new List<bool> ();

		private Vector2 scroll;
		private string baseCategory;

		private void OnEnable ()
		{
			types = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (x => x.GetTypes ()).Where (y => y.IsClass && y.IsPublic).ToArray ();

		}

		private void OnGUI ()
		{
			GUILayout.BeginHorizontal ();
			//GUILayout.Label ("Type", GUILayout.Width (80f));
			selectedTypeString = EditorGUILayout.TextField ("Type", selectedTypeString);

			if (GUILayout.Button ("", "MiniPullDown", GUILayout.Width (17f))) {
				GenericMenu typeMenu = new GenericMenu ();
				for (int i = 0; i < types.Length; i++) {
					Type type = types [i];
					typeMenu.AddItem (new GUIContent (type.FullName.Replace (".", "/")), false, delegate {
						selectedTypeString = type.FullName;
					});
				}
				typeMenu.ShowAsContext ();
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			//GUILayout.Label ("Member", GUILayout.Width (80f));
			selectedMemberInfoString = EditorGUILayout.TextField ("Member", selectedMemberInfoString);

			if (GUILayout.Button ("", "MiniPullDown", GUILayout.Width (17f))) {
				
				Type selectedType = TypeUtility.GetType (selectedTypeString);

				if (selectedType != null) {
					GenericMenu memberMenu = new GenericMenu ();
					MemberInfo[] infos = selectedType.GetMembers (BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
					for (int m = 0; m < infos.Length; m++) {
						MemberInfo member = infos [m];
						if (!member.Name.StartsWith ("get_") && !member.Name.StartsWith ("set_") && !member.Name.StartsWith (".ctor")) {
							memberMenu.AddItem (new GUIContent (member.Name), false, delegate() {
								selectedMemberInfoString = member.Name;
							});
						}
					}
					memberMenu.ShowAsContext ();
				}
			}
			GUILayout.EndHorizontal ();
			baseCategory = EditorGUILayout.TextField ("Base Category", baseCategory);

			Type t = TypeUtility.GetType (selectedTypeString);

			MemberInfo selectedMemberInfo = (t != null && t.GetMember (selectedMemberInfoString).Length > 0 ? t.GetMember (selectedMemberInfoString) [0] : null);
			bool isEnabled = GUI.enabled;
			GUI.enabled = selectedMemberInfo != null;
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Generate")) {
				outputNames.Clear ();
				outputText.Clear ();
				outputSelection.Clear ();
				Generate (t, selectedMemberInfo);
			}
			GUI.enabled = t != null;

			if (GUILayout.Button ("Generate All")) {
				outputNames.Clear ();
				outputText.Clear ();
				outputSelection.Clear ();
				GenerateAll (t);
			}

			GUI.enabled = outputText.Count > 0 && outputSelection.Contains (true);
			if (GUILayout.Button ("Export")) {
				string mPath = EditorUtility.SaveFolderPanel (
					               "Create Action Script",
					               "",
					               "Assets");
				
				for (int i = 0; i < outputText.Count; i++) {

					if (!string.IsNullOrEmpty (mPath) && outputSelection [i]) {
						StreamWriter streamWriter = new System.IO.StreamWriter (mPath + "/" + outputNames [i] + ".cs", false);
						streamWriter.Write (outputText [i]);
						streamWriter.Close ();	
					}

					EditorUtility.DisplayProgressBar ("Progress", "Writing Tasks: " + i + "/" + outputText.Count, (float)i / (float)outputText.Count);
				}
				EditorUtility.ClearProgressBar ();
				AssetDatabase.Refresh ();
			}
			GUILayout.EndHorizontal ();
			GUI.enabled = isEnabled;


			scroll = GUILayout.BeginScrollView (scroll);
			//GUILayout.Label ("Output:");
			GUILayout.BeginVertical ();
			for (int i = 0; i < outputText.Count; i++) {
				GUILayout.BeginHorizontal ();
				outputSelection [i] = EditorGUILayout.Toggle (outputSelection [i], GUILayout.Width (18f));
				GUILayout.Label (outputNames [i] + ".cs");
				GUILayout.EndHorizontal ();

				outputText [i] = GUILayout.TextArea (outputText [i]);
			}

			GUILayout.EndVertical ();
			GUILayout.EndScrollView ();
		}


		private void GenerateAll (Type type)
		{
			MemberInfo[] members = type.GetMembers (BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where (x => !x.Name.StartsWith ("get_") && !x.Name.StartsWith ("set_") && !x.Name.StartsWith (".ctor") && !x.HasAttribute (typeof(ObsoleteAttribute))).ToArray ();
			members = members.GroupBy (x => x.Name).Select (x => x.First ()).ToArray ();
			for (int i = 0; i < members.Length; i++) {
				Generate (type, members [i]);
				EditorUtility.DisplayProgressBar ("Progress", "Generating Tasks: " + i + "/" + members.Length, (float)i / (float)members.Length);
			}
			EditorUtility.ClearProgressBar ();
		}

		private void Generate (Type selectedType, MemberInfo selectedMemberInfo)
		{

			XmlElement documentation = XMLFromMember (selectedMemberInfo);
			List<string> outputLines = new List<string> ();

			if (selectedMemberInfo is MethodInfo) {
				outputLines.AddRange (GenerateBody (selectedType, selectedMemberInfo, documentation, true));
				outputLines.AddRange (GenerateMethod (selectedMemberInfo as MethodInfo, documentation));
				outputLines.Add ("}");
				outputText.Add (ConvertStringArrayToString (outputLines.ToArray ()));
				outputNames.Add (ObjectNames.NicifyVariableName (selectedMemberInfo.Name).Replace (" ", ""));
				outputSelection.Add (true);
			} else if (selectedMemberInfo is PropertyInfo) {
				if ((selectedMemberInfo as PropertyInfo).CanWrite) {
					outputLines.AddRange (GenerateBody (selectedType, selectedMemberInfo, documentation, false));
					outputLines.AddRange (GenerateSetProperty (selectedMemberInfo as PropertyInfo));
					outputLines.Add ("}");
					outputText.Add (ConvertStringArrayToString (outputLines.ToArray ()));
					outputNames.Add ("Set" + ObjectNames.NicifyVariableName (selectedMemberInfo.Name).Replace (" ", ""));
					outputSelection.Add (true);
				}

				outputLines.Clear ();
				if ((selectedMemberInfo as PropertyInfo).CanRead) {
					outputLines.AddRange (GenerateBody (selectedType, selectedMemberInfo, documentation, true));
					outputLines.AddRange (GenerateGetProperty (selectedMemberInfo as PropertyInfo));
					outputLines.Add ("}");
					outputText.Add (ConvertStringArrayToString (outputLines.ToArray ()));
					outputNames.Add (((selectedMemberInfo as PropertyInfo).PropertyType != typeof(bool) ? "Get" : "") + ObjectNames.NicifyVariableName (selectedMemberInfo.Name).Replace (" ", ""));
					outputSelection.Add (true);
				}
			} else if (selectedMemberInfo is FieldInfo) {
				outputLines.AddRange (GenerateBody (selectedType, selectedMemberInfo, documentation, false));
				outputLines.AddRange (GenerateSetField (selectedMemberInfo as FieldInfo));
				outputLines.Add ("}");
				outputText.Add (ConvertStringArrayToString (outputLines.ToArray ()));
				outputNames.Add ("Set" + ObjectNames.NicifyVariableName (selectedMemberInfo.Name).Replace (" ", ""));
				outputSelection.Add (true);

				outputLines.Clear ();

				outputLines.AddRange (GenerateBody (selectedType, selectedMemberInfo, documentation, true));
				outputLines.AddRange (GenerateGetField (selectedMemberInfo as FieldInfo));
				outputLines.Add ("}");
				outputText.Add (ConvertStringArrayToString (outputLines.ToArray ()));
				outputNames.Add (((selectedMemberInfo as FieldInfo).FieldType != typeof(bool) ? "Get" : "") + ObjectNames.NicifyVariableName (selectedMemberInfo.Name).Replace (" ", ""));
				outputSelection.Add (true);
			}
		}

		private List<string> GenerateBody (Type type, MemberInfo info, XmlElement documentation, bool getter)
		{
			List<string> outputLines = new List<string> ();
			outputLines.Add ("using System.Collections;");
			outputLines.Add ("using System.Collections.Generic;");
			if (type.Namespace != "UnityEngine") {
				outputLines.Add ("using UnityEngine;");
			}
			outputLines.Add ("using " + type.Namespace + ";");


			outputLines.Add ("");
			if ((info is MethodInfo && (info as MethodInfo).ReturnType == typeof(bool)) || (info is FieldInfo && (info as FieldInfo).FieldType == typeof(bool) && getter) || (info is PropertyInfo && (info as PropertyInfo).PropertyType == typeof(bool)) && getter) {
				outputLines.Add ("namespace DevionGames.BehaviorTrees.Conditionals.Unity" + type.Name);
			} else {
				outputLines.Add ("namespace DevionGames.BehaviorTrees.Actions.Unity" + type.Name);
			}
			outputLines.Add ("{");
			outputLines.Add ("\t[Category(\"" + (string.IsNullOrEmpty (baseCategory) ? "" : baseCategory + "/") + type.Name + "\")]");
			outputLines.Add ("\t[Tooltip(\"" + (documentation != null ? documentation ["summary"].InnerText.Trim () : "") + "\")]");
			outputLines.Add ("\t[HelpURL(\"https://docs.unity3d.com/ScriptReference/" + type.Name + ((info is MethodInfo) ? "." : "-") + info.Name + ".html\")]");
			return outputLines;
		}

		private List<string> GenerateGetField (FieldInfo property)
		{
			List<string> outputLines = new List<string> ();
			outputLines.Add ("\tpublic class " + (property.FieldType != typeof(bool) ? "Get" : "") + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ": " + (property.FieldType != typeof(bool) ? "Action" : "Conditional") + "{");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t[Tooltip (\"The game object to operate on.\")]");
				outputLines.Add ("\t\tpublic GameObjectVariable m_gameObject;");
			}
			outputLines.Add ("\t\t[Shared]");
			outputLines.Add ("\t\tpublic " + ReplaceVariable (property.FieldType).Name + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ";");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("");
				outputLines.Add ("\t\tprivate GameObject m_PrevGameObject;");
				outputLines.Add ("\t\tprivate " + property.DeclaringType.Name + " m_" + property.DeclaringType.Name + ";");
				outputLines.Add ("");
				outputLines.Add ("\t\tpublic override void OnStart (){");
				outputLines.Add ("\t\t\tif(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){");
				outputLines.Add ("\t\t\t\tm_PrevGameObject=m_gameObject.Value;");
				outputLines.Add ("\t\t\t\tm_" + property.DeclaringType.Name + " = m_gameObject.Value.GetComponent<" + property.DeclaringType.Name + ">();");
				outputLines.Add ("\t\t\t}");
				outputLines.Add ("\t\t}");
			}
			outputLines.Add ("");
			outputLines.Add ("\t\tpublic override TaskStatus OnUpdate (){");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t\tif(" + "m_" + property.DeclaringType.Name + " == null){");
				outputLines.Add ("\t\t\t\tDebug.LogWarning(\"Missing Component of type " + property.DeclaringType.Name + "!\");");
				outputLines.Add ("\t\t\t\treturn TaskStatus.Failure;");
				outputLines.Add ("\t\t\t}");
			}
			outputLines.Add ("\t\t\t" + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ".Value = " + (!typeof(Component).IsAssignableFrom (property.DeclaringType) ? property.DeclaringType.Name : "m_" + property.DeclaringType.Name) + "." + property.Name + ";");
			//outputLines.Add ("\t\t\t" + property.DeclaringType.Name + "." + property.Name + "=" + property.Name + ";");
			outputLines.Add ("\t\t\treturn TaskStatus.Success;");
			outputLines.Add ("\t\t}");

			outputLines.Add ("\t}");
			return outputLines;
		}

		private List<string> GenerateSetField (FieldInfo property)
		{
			List<string> outputLines = new List<string> ();
			outputLines.Add ("\tpublic class Set" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ": Action{");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t[Tooltip (\"The game object to operate on.\")]");
				outputLines.Add ("\t\tpublic GameObjectVariable m_gameObject;");
			}

			outputLines.Add ("\t\tpublic " + ReplaceVariable (property.FieldType).Name + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ";");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("");
				outputLines.Add ("\t\tprivate GameObject m_PrevGameObject;");
				outputLines.Add ("\t\tprivate " + property.DeclaringType.Name + " m_" + property.DeclaringType.Name + ";");
				outputLines.Add ("");
				outputLines.Add ("\t\tpublic override void OnStart (){");
				outputLines.Add ("\t\t\tif(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){");
				outputLines.Add ("\t\t\t\tm_PrevGameObject=m_gameObject.Value;");
				outputLines.Add ("\t\t\t\tm_" + property.DeclaringType.Name + " = m_gameObject.Value.GetComponent<" + property.DeclaringType.Name + ">();");
				outputLines.Add ("\t\t\t}");
				outputLines.Add ("\t\t}");
			}
			outputLines.Add ("");
			outputLines.Add ("\t\tpublic override TaskStatus OnUpdate (){");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t\tif(" + "m_" + property.DeclaringType.Name + " == null){");
				outputLines.Add ("\t\t\t\tDebug.LogWarning(\"Missing Component of type " + property.DeclaringType.Name + "!\");");
				outputLines.Add ("\t\t\t\treturn TaskStatus.Failure;");
				outputLines.Add ("\t\t\t}");
			}
			outputLines.Add ("\t\t\t" + (!typeof(Component).IsAssignableFrom (property.DeclaringType) ? property.DeclaringType.Name : "m_" + property.DeclaringType.Name) + "." + property.Name + " = " + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + (property.FieldType.IsAssignableFrom(typeof(Variable))?".Value":"")+";");
			//outputLines.Add ("\t\t\t" + property.DeclaringType.Name + "." + property.Name + "=" + property.Name + ";");
			outputLines.Add ("\t\t\treturn TaskStatus.Success;");
			outputLines.Add ("\t\t}");

			outputLines.Add ("\t}");
			return outputLines;
		}

		private List<string> GenerateGetProperty (PropertyInfo property)
		{
			List<string> outputLines = new List<string> ();

			outputLines.Add ("\tpublic class " + (property.PropertyType != typeof(bool) ? "Get" : "") + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ": " + (property.PropertyType != typeof(bool) ? "Action" : "Conditional") + "{");

			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t[Tooltip (\"The game object to operate on.\")]");
				outputLines.Add ("\t\tpublic GameObjectVariable m_gameObject;");
			}
			if (property.PropertyType != typeof(bool)) {
				outputLines.Add ("\t\t[Shared]");
				outputLines.Add ("\t\tpublic " + ReplaceVariable (property.PropertyType).Name + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ";");
			}
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("");
				outputLines.Add ("\t\tprivate GameObject m_PrevGameObject;");
				outputLines.Add ("\t\tprivate " + property.DeclaringType.Name + " m_" + property.DeclaringType.Name + ";");
				outputLines.Add ("");
				outputLines.Add ("\t\tpublic override void OnStart (){");
				outputLines.Add ("\t\t\tif(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){");
				outputLines.Add ("\t\t\t\tm_PrevGameObject=m_gameObject.Value;");
				outputLines.Add ("\t\t\t\tm_" + property.DeclaringType.Name + " = m_gameObject.Value.GetComponent<" + property.DeclaringType.Name + ">();");
				outputLines.Add ("\t\t\t}");
				outputLines.Add ("\t\t}");
			}
			outputLines.Add ("");
			outputLines.Add ("\t\tpublic override TaskStatus OnUpdate (){");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t\tif(" + "m_" + property.DeclaringType.Name + " == null){");
				outputLines.Add ("\t\t\t\tDebug.LogWarning(\"Missing Component of type " + property.DeclaringType.Name + "!\");");
				outputLines.Add ("\t\t\t\treturn TaskStatus.Failure;");
				outputLines.Add ("\t\t\t}");
			}
			if (property.PropertyType != typeof(bool)) {
				outputLines.Add ("\t\t\t" + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ".Value = " + (!typeof(Component).IsAssignableFrom (property.DeclaringType) ? property.DeclaringType.Name : "m_" + property.DeclaringType.Name) + "." + property.Name + ";");
				outputLines.Add ("\t\t\treturn TaskStatus.Success;");
			} else {
				outputLines.Add ("\t\t\treturn " + (!typeof(Component).IsAssignableFrom (property.DeclaringType) ? property.DeclaringType.Name : "m_" + property.DeclaringType.Name) + "." + property.Name + " ? TaskStatus.Success : TaskStatus.Failure;");
			}
			outputLines.Add ("\t\t}");

			outputLines.Add ("\t}");
			return outputLines;
		}

		private List<string> GenerateSetProperty (PropertyInfo property)
		{
			List<string> outputLines = new List<string> ();
			outputLines.Add ("\tpublic class Set" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ": Action{");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t[Tooltip (\"The game object to operate on.\")]");
				outputLines.Add ("\t\tpublic GameObjectVariable m_gameObject;");
			}
			outputLines.Add ("\t\tpublic " + ReplaceVariable (property.PropertyType).Name + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + ";");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("");
				outputLines.Add ("\t\tprivate GameObject m_PrevGameObject;");
				outputLines.Add ("\t\tprivate " + property.DeclaringType.Name + " m_" + property.DeclaringType.Name + ";");
				outputLines.Add ("");
				outputLines.Add ("\t\tpublic override void OnStart (){");
				outputLines.Add ("\t\t\tif(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){");
				outputLines.Add ("\t\t\t\tm_PrevGameObject=m_gameObject.Value;");
				outputLines.Add ("\t\t\t\tm_" + property.DeclaringType.Name + " = m_gameObject.Value.GetComponent<" + property.DeclaringType.Name + ">();");
				outputLines.Add ("\t\t\t}");
				outputLines.Add ("\t\t}");
			}
			outputLines.Add ("");
			outputLines.Add ("\t\tpublic override TaskStatus OnUpdate (){");
			if (typeof(Component).IsAssignableFrom (property.DeclaringType)) {
				outputLines.Add ("\t\t\tif(" + "m_" + property.DeclaringType.Name + " == null){");
				outputLines.Add ("\t\t\t\tDebug.LogWarning(\"Missing Component of type " + property.DeclaringType.Name + "!\");");
				outputLines.Add ("\t\t\t\treturn TaskStatus.Failure;");
				outputLines.Add ("\t\t\t}");
			}
			outputLines.Add ("\t\t\t" + (!typeof(Component).IsAssignableFrom (property.DeclaringType) ? property.DeclaringType.Name : "m_" + property.DeclaringType.Name) + "." + property.Name + " = " + " m_" + ObjectNames.NicifyVariableName (property.Name).Replace (" ", "") + (property.PropertyType.IsAssignableFrom(typeof(Variable)) ? ".Value" : "") + ";");
			//outputLines.Add ("\t\t\t" + property.DeclaringType.Name + "." + property.Name + "=" + property.Name + ";");
			outputLines.Add ("\t\t\treturn TaskStatus.Success;");
			outputLines.Add ("\t\t}");

			outputLines.Add ("\t}");
			return outputLines;
		}

		private List<string> GenerateMethod (MethodInfo method, XmlElement documentation)
		{
			ParameterInfo[] variables = method.GetParameters ();

			List<string> outputLines = new List<string> ();
			if (method.ReturnType != typeof(bool)) {
				outputLines.Add ("\tpublic class " + ObjectNames.NicifyVariableName (method.Name).Replace (" ", "") + ": Action");
			} else {
				outputLines.Add ("\tpublic class " + ObjectNames.NicifyVariableName (method.Name).Replace (" ", "") + ": Conditional");
			}
			outputLines.Add ("\t{");
			if (typeof(Component).IsAssignableFrom (method.DeclaringType) && !method.IsStatic) {
				outputLines.Add ("\t\t[Tooltip (\"The game object to operate on.\")]");
				outputLines.Add ("\t\tpublic GameObjectVariable m_gameObject;");
			}
			string variableString = string.Empty;
			if (variables != null) {
				for (int p = 0; p < variables.Length; p++) {
					outputLines.Add ("\t\t[Tooltip(\"" + (documentation != null && documentation ["param"].ParentNode != null && documentation ["param"].ParentNode.ChildNodes.Count > p + 1 ? documentation ["param"].ParentNode.ChildNodes [p + 1].InnerText.Trim () : "") + "\")]");
					outputLines.Add ("\t\tpublic " + ReplaceVariable (variables [p].ParameterType).Name + " " + variables [p].Name + ";");
					variableString += variables [p].Name + (variables [p].ParameterType == typeof(string) ? ".Value" : "") + ", ";
				}
			}
			if (method.ReturnType != typeof(void) && method.ReturnType != typeof(bool)) {
				outputLines.Add ("\t\t[Shared]");
				outputLines.Add ("\t\tpublic " + ReplaceVariable (method.ReturnType).Name + " m_" + method.Name + ";");
			}

			variableString = variableString.TrimEnd (',', ' ');
			if (typeof(Component).IsAssignableFrom (method.DeclaringType) && !method.IsStatic) {
				outputLines.Add ("");
				outputLines.Add ("\t\tprivate GameObject m_PrevGameObject;");
				outputLines.Add ("\t\tprivate " + method.DeclaringType.Name + " m_" + method.DeclaringType.Name + ";");
				outputLines.Add ("");
				outputLines.Add ("\t\tpublic override void OnStart ()");
				outputLines.Add ("\t\t{");
				outputLines.Add ("\t\t\tif(m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject){");
				outputLines.Add ("\t\t\t\tm_PrevGameObject=m_gameObject.Value;");
				outputLines.Add ("\t\t\t\tm_" + method.DeclaringType.Name + " = m_gameObject.Value.GetComponent<" + method.DeclaringType.Name + ">();");
				outputLines.Add ("\t\t\t}");
				outputLines.Add ("\t\t}");
			}
			outputLines.Add ("");
			outputLines.Add ("\t\tpublic override TaskStatus OnUpdate ()");
			outputLines.Add ("\t\t{");
			if (typeof(Component).IsAssignableFrom (method.DeclaringType) && !method.IsStatic) {
				outputLines.Add ("\t\t\tif(" + "m_" + method.DeclaringType.Name + " == null)");
				outputLines.Add ("\t\t\t{");
				outputLines.Add ("\t\t\t\tDebug.LogWarning(\"Missing Component of type " + method.DeclaringType.Name + "!\");");
				outputLines.Add ("\t\t\t\treturn TaskStatus.Failure;");
				outputLines.Add ("\t\t\t}");
			}
			if (method.ReturnType != typeof(bool)) {
				outputLines.Add ("\t\t\t" + (method.ReturnType != typeof(void) ? "m_" + method.Name + ".Value = " : "") + (method.IsStatic ? method.DeclaringType.Name : "m_" + method.DeclaringType.Name) + "." + method.Name + "(" + variableString + ");");
				outputLines.Add ("\t\t\treturn TaskStatus.Success;");
			} else {
				outputLines.Add ("\t\t\treturn " + (method.IsStatic ? method.DeclaringType.Name : "m_" + method.DeclaringType.Name) + "." + method.Name + "(" + variableString + ") ? TaskStatus.Success : TaskStatus.Failure;");
			}
			outputLines.Add ("\t\t}");

			outputLines.Add ("\t}");
			return outputLines;
		}

		private string ReplaceVariable (string line)
		{
			if (string.IsNullOrEmpty (line)) {
				return line;
			}
			if (line.Contains ("float") || line.Contains ("Single") || line.Contains ("Double")) {
				line = line.Replace ("float", "FloatVariable");
				line = line.Replace ("Single", "FloatVariable");
				line = line.Replace ("Double", "FloatVariable");
			} else if (line.Contains ("string") || line.Contains ("String")) {
				line = line.Replace ("string", "StringVariable");
				line = line.Replace ("String", "StringVariable");
			} else if (line.Contains ("Color")) {
				line = line.Replace ("Color", "ColorVariable");
			} else if (line.Contains ("GameObject")) {
				line = line.Replace ("GameObject", "GameObjectVariable");
			} else if (line.Contains ("int") || line.Contains ("Int32")) {
				line = line.Replace ("int", "IntVariable");
				line = line.Replace ("Int32", "IntVariable");
			} else if (line.Contains ("bool") || line.Contains ("Boolean")) {
				line = line.Replace ("bool", "BoolVariable");
				line = line.Replace ("Boolean", "BoolVariable");
			} else if (line.Contains ("Vector2")) {
				line = line.Replace ("Vector2", "Vector2Variable");
			} else if (line.Contains ("Vector3")) {
				line = line.Replace ("Vector3", "Vector3Variable");
			} else if (line.Contains ("Vector4")) {
				line = line.Replace ("Vector4", "Vector4Variable");
			} else if (line.Contains ("Transform")) {
				line = line.Replace ("Transform", "TransformVariable");
			} else if (line.Contains ("Material")) {
				line = line.Replace ("Material", "MaterialVariable");
			} else if (line.Contains ("Object")) {
				line = line.Replace ("Object", "ObjectVariable");
			}
			return line;
		}

		private Type ReplaceVariable (Type type)
		{
			if (type == typeof(bool)) {
				return typeof(BoolVariable); 
			} else if (type == typeof(float) || type == typeof(double)) {
				return typeof(FloatVariable);
			} else if (type == typeof(string)) {
				return typeof(StringVariable);
			} else if (type == typeof(GameObject)) {
				return typeof(GameObject);
			} else if (type == typeof(int)) {
				return typeof(IntVariable);
			} else if (type == typeof(Color)) {
				return typeof(ColorVariable);
			} else if (type == typeof(Material)) {
				return typeof(MaterialVariable);
			} else if (type == typeof(UnityEngine.Object)) {
				return typeof(ObjectVariable);
			} else if (type == typeof(Transform)) {
				return typeof(Transform);
			} else if (type == typeof(Vector2)) {
				return typeof(Vector2Variable);
			} else if (type == typeof(Vector3)) {
				return typeof(Vector3Variable);
			} else if (type == typeof(Vector4)) {
				return typeof(Vector4Variable);
			} 
			return type;
		}

		private bool IsConvertableVariable (Type type)
		{
			if (type == typeof(bool)) {
				return true; 
			} else if (type == typeof(float) || type == typeof(double)) {
				return true; 
			} else if (type == typeof(string)) {
				return true; 
			} else if (type == typeof(GameObject)) {
				return true; 
			} else if (type == typeof(int)) {
				return true; 
			} else if (type == typeof(Color)) {
				return true; 
			} else if (type == typeof(Material)) {
				return true; 
			} else if (type == typeof(UnityEngine.Object)) {
				return true; 
			} else if (type == typeof(Transform)) {
				return true; 
			} else if (type == typeof(Vector2)) {
				return true; 
			} else if (type == typeof(Vector3)) {
				return true; 
			} else if (type == typeof(Vector4)) {
				return true; 
			} 
			return false;
		}

		private string ConvertStringArrayToString (string[] array)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder ();
			foreach (string value in array) {
				builder.Append (value);
				builder.Append ('\n');
			}
			return builder.ToString ();
		}

		public static XmlElement XMLFromMember (MethodInfo methodInfo)
		{
			// Calculate the parameter string as this is in the member name in the XML
			string parametersString = "";
			foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
				if (parametersString.Length > 0) {
					parametersString += ",";
				}

				parametersString += parameterInfo.ParameterType.FullName;
			}

			//AL: 15.04.2008 ==> BUG-FIX remove “()” if parametersString is empty
			if (parametersString.Length > 0)
				return XMLFromName (methodInfo.DeclaringType, 'M', methodInfo.Name + "(" + parametersString + ")");
			else
				return XMLFromName (methodInfo.DeclaringType, 'M', methodInfo.Name);
		}

		public static XmlElement XMLFromMember (MemberInfo memberInfo)
		{
			// First character [0] of member type is prefix character in the name in the XML
			return XMLFromName (memberInfo.DeclaringType, memberInfo.MemberType.ToString () [0], memberInfo.Name);
		}

		private static XmlElement XMLFromName (Type type, char prefix, string name)
		{
			string fullName;

			if (String.IsNullOrEmpty (name)) {
				fullName = prefix + ":" + type.FullName;
			} else {
				fullName = prefix + ":" + type.FullName + "." + name;
			}

			XmlDocument xmlDocument = XMLFromAssembly (type.Assembly);
			if (xmlDocument == null) {
				return null;
			}
			XmlElement matchedElement = null;

			foreach (XmlNode xmlElement in xmlDocument["doc"]["members"].ChildNodes) {
				if (xmlElement.OuterXml.Contains (fullName)) {
					matchedElement = (XmlElement)xmlElement;
					break;
				}
			}
			return matchedElement;
		}

		private static XmlDocument XMLFromAssembly (Assembly assembly)
		{
			string assemblyFilename = assembly.CodeBase;

			const string prefix = "file:///";

			if (assemblyFilename.StartsWith (prefix)) {
				StreamReader streamReader;

				try {
					streamReader = new StreamReader (Path.ChangeExtension (assemblyFilename.Substring (prefix.Length), ".xml"));
				} catch (FileNotFoundException exception) {
					Debug.Log ("XML documentation not present! " + exception.ToString ());
					return null;
				}

				XmlDocument xmlDocument = new XmlDocument ();
				xmlDocument.Load (streamReader);
				return xmlDocument;
			} else {
				Debug.Log ("Could not ascertain assembly filename.");
			}
			return null;
		}
	}
}