using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public static class GUIDrawer
	{
		private static Dictionary<Type,ObjectDrawer> drawers;
		private static Dictionary<Type,FieldInfo[]> fieldsLookup;

		static GUIDrawer ()
		{
			RebuildDrawers ();
		}

		private static void RebuildDrawers ()
		{
			GUIDrawer.drawers = new Dictionary<Type, ObjectDrawer> ();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies ();
			for (int i = 0; i < (int)assemblies.Length; i++) {
				Assembly assembly = assemblies [i];
				if (assembly is System.Reflection.Emit.AssemblyBuilder) continue;

				Type[] types = assembly.GetExportedTypes ();
				for (int j = 0; j < (int)types.Length; j++) {
					Type type = types [j];
					if (typeof(ObjectDrawer).IsAssignableFrom (type) && type.IsClass && !type.IsAbstract) {
						Type inspectedType = GetInspectedType (type);
						if (inspectedType != null && !GUIDrawer.drawers.ContainsKey (inspectedType)) {
							ObjectDrawer drawer = (ObjectDrawer)Activator.CreateInstance (type);
							GUIDrawer.drawers.Add (inspectedType, drawer);
							Type[] subTypes = TypeUtility.GetTypes ().Where (x => x.IsSubclassOf (inspectedType)).ToArray ();
				
							for (int k = 0; k < subTypes.Length; k++) {
								if (!GUIDrawer.drawers.ContainsKey (subTypes [k])) {
									GUIDrawer.drawers.Add (subTypes [k], drawer);
								}
							}
						}
					}
				}
			}		
		}

		public static ObjectDrawer GetDrawer (FieldInfo field)
		{
			return GetDrawer (field.FieldType);
		}

		public static ObjectDrawer GetDrawer (Type type)
		{
			if (type == null) {
				return  null;			
			}
			ObjectDrawer drawer;
			GUIDrawer.drawers.TryGetValue (type, out drawer);
			return drawer;
		}

		public static Type GetInspectedType (Type type)
		{
			object[] objArray = type.GetCustomAttributes (true);
			for (int i = 0; i < (int)objArray.Length; i++) {
				CustomObjectDrawerAttribute drawerAttribute = objArray [i] as CustomObjectDrawerAttribute;
				if (drawerAttribute != null) {
					return drawerAttribute.Type;
				}
			}
			return null;
		}
	}
}