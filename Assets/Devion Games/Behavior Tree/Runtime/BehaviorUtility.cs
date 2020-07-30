using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;


namespace DevionGames.BehaviorTrees
{
	public static class BehaviorUtility
	{

		public static Behavior GetBehavior(this GameObject gameObject, string behaviorName) {
			Behavior[] behaviors = gameObject.GetComponents<Behavior>();
			for (int i = 0; i < behaviors.Length; i++) {
				if (behaviors[i].GetBehaviorTree().name == behaviorName) {
					return behaviors[i];
				}
			}
			return null;
		}

		public static string Save (BehaviorTree behaviorTree)
		{
			if (behaviorTree == null) {
				return string.Empty;
			}
			behaviorTree.serializationData = string.Empty;
			behaviorTree.serializedObjects = new List<UnityEngine.Object> ();
			behaviorTree.variables = new List<VariableReference> ();

			Task[] nodes = BehaviorUtility.GetAllNodes (behaviorTree);
			for (int i = 0; i < nodes.Length; i++) {
				nodes [i].nodeInfo.id = i;
			}
			List<UnityEngine.Object> objects = new List<UnityEngine.Object> ();
			Dictionary<string,object> data = new Dictionary<string, object> ();
			if (behaviorTree.root != null) {
				data.Add ("EntryTask", SerializeTask (behaviorTree, behaviorTree.root, ref objects));
			}
			if (behaviorTree.detachedNodes.Count > 0) {
				Dictionary<string, object>[] dic = new Dictionary<string, object>[behaviorTree.detachedNodes.Count];
				for (int i = 0; i < behaviorTree.detachedNodes.Count; i++) {
					dic [i] = SerializeTask (behaviorTree, behaviorTree.detachedNodes [i], ref objects);
				}
				data.Add ("DetachedTasks", dic);
			}
	
			behaviorTree.serializationData = MiniJSON.Serialize (data);
			behaviorTree.serializedObjects = objects;
			return behaviorTree.serializationData;
		}

		private static Dictionary<string, object> SerializeTask (BehaviorTree behaviorTree, Task task, ref List<UnityEngine.Object> objects)
		{
			Dictionary<string, object> dic = new Dictionary<string, object> () {
				{ "Type", task.GetType () },
			};

			SerializeFields (task, ref dic, ref objects);

			if (task.children.Count > 0) {
				Dictionary<string, object>[] dictionaryArrays = new Dictionary<string, object>[task.children.Count];
				for (int i = 0; i < task.children.Count; i++) {
					dictionaryArrays [i] = SerializeTask (behaviorTree, task.children [i], ref objects);
				}
				dic.Add ("Children", dictionaryArrays);
			}	
			return dic;
		}

		private static void SerializeFields (object obj, ref Dictionary<string,object> dic, ref List<UnityEngine.Object> unityObjects)
		{
			if (obj == null) {
				return;
			}
			Type type = obj.GetType ();
			FieldInfo[] fields = type.GetAllFields ();

			for (int j = 0; j < fields.Length; j++) {
				FieldInfo field = fields [j];
				object value = field.GetValue (obj);
				SerializeValue (field.Name, value, ref dic, ref unityObjects);
			
			}
		}
		//	SerializeField (behaviorTree, obj, field, ref dic, ref unityObjects);

		private static void SerializeValue (string key, object value, ref Dictionary<string,object> dic, ref List<UnityEngine.Object> unityObjects)
		{
			if (value != null && !dic.ContainsKey(key)) {
				Type type = value.GetType ();

				if (typeof(UnityEngine.Object).IsAssignableFrom (type)) {
					UnityEngine.Object unityObject = value as UnityEngine.Object;
					if (!unityObjects.Contains (unityObject)) {
						unityObjects.Add (unityObject);
					}
					dic.Add (key, unityObjects.IndexOf (unityObject));
				} else if (typeof(LayerMask).IsAssignableFrom (type)) {
					dic.Add (key, ((LayerMask)value).value);
				} else if (typeof(Enum).IsAssignableFrom (type)) {
					dic.Add (key, (Enum)value);
				} else if (type.IsPrimitive ||
				           type == typeof(string) ||
				           type == typeof(Vector2) ||
				           type == typeof(Vector3) ||
				           type == typeof(Vector4) ||
				           type == typeof(Color)) {
					dic.Add (key, value);
				} else if (typeof(IList).IsAssignableFrom (type)) {
					IList list = (IList)value;
					Dictionary<string,object> s = new Dictionary<string, object> ();
					for (int i = 0; i < list.Count; i++) {
						SerializeValue (i.ToString (), list [i], ref s, ref unityObjects);
					}
					dic.Add (key, s);
				} else {
					Dictionary<string,object> data = new Dictionary<string, object> ();
					SerializeFields (value, ref data, ref unityObjects);
					dic.Add (key, data);
				}
			}
		}

		public static void Load (BehaviorTree behaviorTree)
		{
			if (behaviorTree == null || string.IsNullOrEmpty (behaviorTree.serializationData)) {
				return;
			}
			Dictionary<string, object> data = MiniJSON.Deserialize (behaviorTree.serializationData) as Dictionary<string, object>;
			behaviorTree.variables = new List<VariableReference> ();
			object obj;
			if (data.TryGetValue ("EntryTask", out obj)) {
				behaviorTree.root = DeserializeTask (behaviorTree, obj as Dictionary<string,object>, behaviorTree.serializedObjects);
			}
			behaviorTree.detachedNodes.Clear ();
			if (data.TryGetValue ("DetachedTasks", out obj)) {
				List<object> list = obj as List<object>;
				for (int i = 0; i < list.Count; i++) {
					behaviorTree.detachedNodes.Add (DeserializeTask (behaviorTree, list [i] as Dictionary<string,object>, behaviorTree.serializedObjects));
				}
			}
		}

		private static Task DeserializeTask (BehaviorTree behaviorTree, Dictionary<string,object> data, List<UnityEngine.Object> unityObjects)
		{
			string typeString = (string)data ["Type"];
			Type type = TypeUtility.GetType (typeString);
			if (type == null && !string.IsNullOrEmpty (typeString)) {
				type = TypeUtility.GetType (typeString.Split ('.').Last ());
			}
			Task task = (Task)System.Activator.CreateInstance (type);
			DeserializeFields (behaviorTree, task, data, unityObjects);

			object obj;
			if (data.TryGetValue ("Children", out obj)) {
				List<object> list = obj as List<object>;
				for (int i = 0; i < list.Count; i++) {
					task.Add (DeserializeTask (behaviorTree, list [i] as Dictionary<string,object>, unityObjects));
				}
			}
			return task;
		}

		private static void DeserializeFields (BehaviorTree behaviorTree, object obj, Dictionary<string,object> data, List<UnityEngine.Object> unityObjects)
		{
			if (obj == null) {
				return;
			}
			Type type = obj.GetType ();
			FieldInfo[] fields = type.GetAllFields ();

			for (int j = 0; j < fields.Length; j++) {
				FieldInfo field = fields [j];
				object value = DeserializeValue (field.Name, behaviorTree, obj, field, field.FieldType, data, unityObjects);
				if (value != null) {
					field.SetValue (obj, value);
				}
			}
		}


		private static Type GetElementType (Type type)
		{
			Type[] interfaces = type.GetInterfaces ();

			return (from i in interfaces
			        where i.IsGenericType && i.GetGenericTypeDefinition () == typeof(IEnumerable<>)
			        select i.GetGenericArguments () [0]).FirstOrDefault ();
		}

		private static object DeserializeValue (string key, BehaviorTree behaviorTree, object declaringObject, FieldInfo field, Type type, Dictionary<string,object> data, List<UnityEngine.Object> unityObjects)
		{
			object value;
			if (data.TryGetValue (key, out value)) {
				if (typeof(UnityEngine.Object).IsAssignableFrom (type)) {
					int index = System.Convert.ToInt32 (value);
					if (index >= 0 && index < unityObjects.Count) {
						return unityObjects [index];
					}
				} else if (typeof(LayerMask) == type) {
					LayerMask mask = new LayerMask ();
					mask.value = (int)value;
					return mask;
				} else if (typeof(Enum).IsAssignableFrom (type)) {
					return Enum.Parse (type, (string)value);
				} else if (type.IsPrimitive ||
				           type == typeof(string) ||
				           type == typeof(Vector2) ||
				           type == typeof(Vector3) ||
				           type == typeof(Vector4) ||
				           type == typeof(Quaternion) ||
				           type == typeof(Color)) {

					return value;
				} else if (typeof(IList).IsAssignableFrom (type)) {
					Dictionary<string,object> dic = value as Dictionary<string,object>;
			
					Type targetType = typeof(List<>).MakeGenericType (GetElementType (type));
					IList result = (IList)Activator.CreateInstance (targetType);
					int count = dic.Count;
					for (int i = 0; i < count; i++) {
						result.Add (DeserializeValue (i.ToString (), behaviorTree, declaringObject, field, GetElementType (type), dic, unityObjects));
					}
					//Missing Array/List converts
					if (type.IsArray) {
						Array array = Array.CreateInstance (GetElementType (type), count);
						result.CopyTo (array, 0);
						return array;
					}
					return result;
				} else {
					object instance = Activator.CreateInstance (type);
					if (typeof(Variable).IsAssignableFrom (type) && !typeof(GenericVariable).IsAssignableFrom (type)) {
						Variable variable = (Variable)instance;
						if (variable.isNone) {
							Debug.Log (key + " " + (variable as GenericVariable).isNone);
							return null;
						}
						//Debug.Log(field.Name+" "+((Variable)instance).type);
						behaviorTree.variables.Add (new VariableReference (field, declaringObject, (Variable)instance));
					}

					DeserializeFields (behaviorTree, instance, value as Dictionary<string,object>, unityObjects);
					return instance;
				}
			}
			return null;
		}

		public static object ConvertToArray (this IList collection)
		{
			// guess type
			Type type;
			if (collection.GetType ().IsGenericType && collection.GetType ().GetGenericArguments ().Length == 0)
				type = collection.GetType ().GetGenericArguments () [0];
			else if (collection.Count > 0)
				type = collection [0].GetType ();
			else
				throw new NotSupportedException ("Failed to identify collection type for: " + collection.GetType ());

			var array = (object[])Array.CreateInstance (type, collection.Count);
			for (int i = 0; i < array.Length; ++i)
				array [i] = collection [i];
			return array;
		}

		public static object ConvertToArray (this IList collection, Type arrayType)
		{
			var array = (object[])Array.CreateInstance (arrayType, collection.Count);
			for (int i = 0; i < array.Length; ++i) {
				var obj = collection [i];

				// if it's not castable, try to convert it
				if (!arrayType.IsInstanceOfType (obj))
					obj = Convert.ChangeType (obj, arrayType);

				array [i] = obj;
			}

			return array;
		}

		public static bool HasConfigurationErrors (IBehavior behavior)
		{
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
						bool required = !AttributeUtility.HasAttribute (fields [f], typeof(NotRequiredAttribute));
						if ((!variable.isShared && required && typeof(UnityEngine.Object).IsAssignableFrom (variable.type) && ((UnityEngine.Object)variable.RawValue) == null) || (variable.isShared && required && (variable.isNone || behavior.GetBehaviorTree ().blackboard.GetVariable (variable.name, true) == null))) {
							return true;
						}

					}
				}

				if (node.maxChildCount > 0 && (node.children.Count == 0 || node.children.Where (x => x.enabled).ToArray ().Length == 0)) {
					return true;
				}
			}
			return false;
		}

		public static Task AddNode (BehaviorTree behaviorTree, Task parent, System.Type type, Vector2 position)
		{
			Task node = (Task)System.Activator.CreateInstance (type);
			node.nodeInfo.position = position;

			#if UNITY_EDITOR
			node.name = UnityEditor.ObjectNames.NicifyVariableName (type.Name);
			#endif

			if (parent != null && parent.maxChildCount > parent.children.Count) {
				parent.Add (node);
			} else {
				behaviorTree.detachedNodes.Add (node);
			}
			FieldInfo[] fields = node.GetType ().GetFields ();
			for (int i = 0; i < fields.Length; i++) {
				FieldInfo field = fields [i];
				object value = field.GetValue (node);
				if (!field.HasAttribute (typeof(SharedAttribute)) && field.FieldType == typeof(GameObjectVariable)) {
					Variable variable = value as Variable;
					if (variable == null) {
						variable = new GameObjectVariable ();
						field.SetValue (node, variable);
					}
					variable.isShared = true;
					variable.name = "Self";
				}
			}
			return node;
		}

		public static Variable[] GetReferencedVariables (Blackboard blackboard, Task[] nodes)
		{
			List<Variable> referenced = new List<Variable> ();
			for (int i = 0; i < nodes.Length; i++) {
				referenced.AddRange (GetReferencedVariables (blackboard, nodes [i]));
			}
			return referenced.ToArray ();
		}

		public static Variable[] GetReferencedVariables (Blackboard blackboard, Task task)
		{
			List<Variable> referenced = new List<Variable> ();
			FieldInfo[] fields = task.GetType ().GetAllFields ();
			for (int i = 0; i < fields.Length; i++) {
				FieldInfo field = fields [i];
				if (typeof(Variable).IsAssignableFrom (field.FieldType)) {
					Variable variable = field.GetValue (task) as Variable;
					if (variable != null) {
						Variable blackboardVar = blackboard.GetVariable (variable.name);
						if (variable.isShared && !variable.isNone && blackboardVar != null ) {
							referenced.Add (blackboardVar);
						}
					}
				}
			}
			return referenced.ToArray ();
		}

		public static void DeleteNodes (BehaviorTree behaviorTree, Task[] nodes)
		{
			for (int i = 0; i < nodes.Length; i++) {
				Task node = nodes [i];
				if (node.parent != null) {
					node.parent.Remove (node);
				} //else {
				behaviorTree.detachedNodes.Remove (node);	
				//}
				behaviorTree.detachedNodes.AddRange (node.children);
				node.Clear ();
			}
		}

		public static void OffsetNode (Task node, Vector2 offset, bool includeChildren = true)
		{
			node.nodeInfo.position += offset;
			for (int i = 0; i < node.children.Count; i++) {
				OffsetNode (node.children [i], offset, includeChildren);
			}
		}

		public static bool SelfOrParentDisabled (Task node)
		{
			if (!node.enabled) {
				return true;
			}
			Task parent = node.parent;
			while (parent != null) {
				if (parent != null && !parent.enabled) {
					return true;
				}
				parent = parent.parent;
			}
			return false;
		}

		public static Task[] GetAllActiveNodes (BehaviorTree behaviorTree)
		{
			List<Task> nodeList = new List<Task> ();
			if (behaviorTree != null) {
				GetNodesRecursive (behaviorTree.root, ref nodeList, true);
			}
			return nodeList.Where (x => !SelfOrParentDisabled (x)).ToArray ();
		}

		public static Task[] GetAllNodes (IBehavior behavior, bool includeCollapsed = true)
		{
			return GetAllNodes (behavior.GetBehaviorTree (), includeCollapsed);
		}

		public static Task[] GetAllNodes (BehaviorTree behaviorTree, bool includeCollapsed = true)
		{
			List<Task> nodeList = new List<Task> ();
			if (behaviorTree != null) {
				GetNodesRecursive (behaviorTree.root, ref nodeList, includeCollapsed);
				for (int i = 0; i < behaviorTree.detachedNodes.Count; i++) {
					GetNodesRecursive (behaviorTree.detachedNodes [i], ref nodeList, includeCollapsed);
				}
			}
			return nodeList.ToArray ();
		}

		public static void GetNodesRecursive (Task node, ref List<Task> nodeList, bool includeCollapsed = true)
		{
			if (node != null) {
				nodeList.Add (node);
				if (includeCollapsed || !node.nodeInfo.isCollapsed) {
					for (int i = 0; i < node.children.Count; i++) {
						GetNodesRecursive (node.children [i], ref nodeList, includeCollapsed);
					}
				}
			}
		}
	}
}