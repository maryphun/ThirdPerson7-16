using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames.BehaviorTrees
{
	public static class ErrorChecker
	{
		private static readonly List<TaskError> errorList;
		private static bool checkForErrors = true;

		static ErrorChecker ()
		{
			ErrorChecker.errorList = new List<TaskError> ();
			EditorApplication.update += Update;
		}

		public static void CheckForErrors ()
		{
			ClearErrors ();
			checkForErrors = true;
		}

		public static void CheckForErrors (IBehavior behavior)
		{
			if (behavior == null) {
				return;
			}
			RemoveErrors (behavior);
			Task[] nodes = BehaviorUtility.GetAllActiveNodes (behavior.GetBehaviorTree ());

			for (int i = 0; i < nodes.Length; i++) {
				Task node = nodes [i];
				FieldInfo[] fields = ReflectionUtility.GetFields (node.GetType ());
				for (int f = 0; f < fields.Length; f++) {
			
					if (typeof(Variable).IsAssignableFrom (fields [f].FieldType)) {
						object obj = fields [f].GetValue (node);
						if (obj == null) {
							obj = Activator.CreateInstance (fields [f].FieldType);
							fields [f].SetValue (node, obj);
						}
						Variable variable = obj as Variable;
						TaskError error = new TaskError (behavior, node, variable, TaskError.ErrorType.RequiredField);
						bool required = !AttributeUtility.HasAttribute (fields [f], typeof(NotRequiredAttribute));
					
						if ((!variable.isShared && required && typeof(UnityEngine.Object).IsAssignableFrom (variable.type) && ((UnityEngine.Object)variable.RawValue) == null) || (variable.isShared && required && (variable.isNone || behavior.GetBehaviorTree ().blackboard.GetVariable (variable.name, true) == null))) {

							if (!ContainsError (error)) {
								errorList.Add (error);
							}
						}

					}
				}

				if (node.maxChildCount > 0 && (node.children.Count == 0 || node.children.Where (x => x.enabled).ToArray ().Length == 0)) {
					TaskError error = new TaskError (behavior, node, TaskError.ErrorType.RequiredChild);
					if (!ContainsError (error)) {
						errorList.Add (error);
					}
				}
			}
			EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow> ();
			foreach (EditorWindow window in windows) {
				window.Repaint ();
			}
			checkForErrors = false;
		}

		public static bool HasErrors (Task node)
		{
			foreach (TaskError mError in errorList) {
				if (mError.node == node) {
					return true;
				}		
			}
			return false;
		}

		public static bool HasErrors (IBehavior behavior)
		{
			return GetError (behavior) != null;
		}

		public static TaskError GetError (IBehavior behavior)
		{
			foreach (TaskError mError in errorList) {
				if (mError.behavior == behavior) {
					return mError;
				}		
			}	
			return null;
		}

		public static TaskError[] GetErrors (IBehavior behavior)
		{
			List<TaskError> errors = new List<TaskError> ();
			foreach (TaskError mError in errorList) {
				if (mError.behavior == behavior) {
					errors.Add (mError);
				}		
			}	
			return errors.ToArray ();
		}

		public static void RemoveErrors (IBehavior behavior)
		{
			TaskError[] errors = GetErrors (behavior);
			foreach (TaskError error in errors) {
				errorList.Remove (error);
			}
		}

		public static void ClearErrors ()
		{
			errorList.Clear ();		
		}

		private static bool CheckForVariableError (Variable variable, FieldInfo field)
		{
			if (variable.isShared) {
				return variable.isNone;			
			} 
			return false;	
		}

		private static bool ContainsError (TaskError error)
		{
			foreach (TaskError mError in errorList) {
				if (mError.SameAs (error)) {
					return true;
				}
			}	
			return false;
		}

		public static List<TaskError> GetErrors ()
		{
			return ErrorChecker.errorList;
		}

		public static void Update ()
		{
			if (checkForErrors) {
				IBehavior[] behaviors = FindBehaviors ();
				for (int i = 0; i < behaviors.Length; i++) {
					ErrorChecker.CheckForErrors (behaviors [i]);	
				}
			}
		}

		private static IBehavior[] FindBehaviors ()
		{
			IBehavior[] behaviors = Resources.FindObjectsOfTypeAll<UnityEngine.Object> ().Where (x => typeof(IBehavior).IsAssignableFrom (x.GetType ())).Select (y => y as IBehavior).ToArray ();
			return behaviors;
		}


	}
}