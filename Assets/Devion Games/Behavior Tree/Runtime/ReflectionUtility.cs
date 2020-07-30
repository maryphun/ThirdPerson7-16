using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public static class ReflectionUtility
	{
		private static Dictionary<Type,FieldInfo[]> fieldsLookup;

		static ReflectionUtility ()
		{
			fieldsLookup = new Dictionary<Type, FieldInfo[]> ();
		}

		public static FieldInfo[] GetAllFields (this Type type)
		{
			if (type == null) {
				return new FieldInfo[0];
			}
			FieldInfo[] fields = GetFields (type).Concat (GetAllFields (type.BaseType)).ToArray ();
			fields = fields.OrderBy (x => x.DeclaringType.BaseTypesAndSelf ().Count ()).ToArray ();
			return fields;
		}

		public static FieldInfo[] GetFields (Type type)
		{
			FieldInfo[] fields;
			if (!fieldsLookup.TryGetValue (type, out fields)) {
				fields = type.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where (x => x.IsPublic && !x.HasAttribute (typeof(NonSerializedAttribute)) || x.HasAttribute (typeof(SerializeField))).ToArray ();
				fieldsLookup.Add (type, fields);
			}
			return fields;
		}

		public static IEnumerable<Type> BaseTypesAndSelf (this Type type)
		{
			while (type != null) {
				yield return type;
				type = type.BaseType;
			}
		}
	}
}