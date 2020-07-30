using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames.BehaviorTrees
{
	public static class TypeUtility
	{
		private static Assembly[] assembliesLookup;
		private static Dictionary<string, Type> typeLookup;

		static TypeUtility ()
		{
			assembliesLookup = TypeUtility.GetLoadedAssemblies ();
			// Remove Editor assemblies
			var runtimeAsms = new List<Assembly> ();
			foreach (Assembly asm in assembliesLookup) {
				if (!asm.GetName ().Name.Contains ("Editor"))
					runtimeAsms.Add (asm);
			}
			assembliesLookup = runtimeAsms.ToArray ();
			typeLookup = new Dictionary<string, Type> ();

		}

		public static Assembly[] GetLoadedAssemblies ()
		{
			#if NETFX_CORE
			var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

			List<Assembly> loadedAssemblies = new List<Assembly>();

			var folderFilesAsync = folder.GetFilesAsync();
			folderFilesAsync.AsTask().Wait();

			foreach (var file in folderFilesAsync.GetResults())
			{
			if (file.FileType == ".dll" || file.FileType == ".exe")
			{
			try
			{
			var filename = file.Name.Substring(0, file.Name.Length - file.FileType.Length);
			AssemblyName name = new AssemblyName { Name = filename };
			Assembly asm = Assembly.Load(name);
			loadedAssemblies.Add(asm);
			}
			catch (BadImageFormatException)
			{
			// Thrown reflecting on C++ executable files for which the C++ compiler stripped the relocation addresses (such as Unity dlls): http://msdn.microsoft.com/en-us/library/x4cw969y(v=vs.110).aspx
			}
			}
			}

			return loadedAssemblies.ToArray();
			#else
			return AppDomain.CurrentDomain.GetAssemblies ();
			#endif
		}



		public static Type GetType (string typeName)
		{
			Type type;
			if (typeLookup.TryGetValue (typeName, out type)) {
				return type;
			}
			type = Type.GetType (typeName);
			if (type == null) {
				int num = 0;
				while (num < assembliesLookup.Length) {
					type = Type.GetType (string.Concat (typeName, ",", assembliesLookup [num].FullName));
					if (type == null) {
						num++;
					} else {
						break;
					}
				}
			}

			if (type == null) {
				foreach (Assembly a in assembliesLookup) {
					Type[] assemblyTypes = a.GetTypes ();
					for (int j = 0; j < assemblyTypes.Length; j++) {
						if (assemblyTypes [j].Name == typeName) {
							type = assemblyTypes [j];
							break;
						}
					}
				}
			}

			if (type != null) {
				typeLookup.Add (typeName, type);
			}

			return type;
		}

		public static Type[] GetTypes ()
		{
			return assembliesLookup.SelectMany (x => x.GetTypes ()).ToArray ();
		}
	}
}