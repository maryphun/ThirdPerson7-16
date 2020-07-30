using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class Toolbar
	{
		public delegate T ActionByRef<T> (ref T value);

		private List<ToolbarItem> items;
		public ToolbarItemPosition itemAlignment;

		public Toolbar ()
		{
			items = new List<ToolbarItem> ();
			itemAlignment = ToolbarItemPosition.Left;
		}

		public void OnGUI ()
		{
			GUIStyle style = new GUIStyle (EditorStyles.toolbar);
			style.padding.left = 0;
			GUILayout.BeginHorizontal (style);
			if (itemAlignment == ToolbarItemPosition.Right || itemAlignment == ToolbarItemPosition.Center) {
				GUILayout.FlexibleSpace ();
			}
			for (int i = 0; i < items.Count; i++) {
				ToolbarItem item = items [i];
				item.OnGUI ();
			}
			if (itemAlignment == ToolbarItemPosition.Left || itemAlignment == ToolbarItemPosition.Center) {
				GUILayout.FlexibleSpace ();
			}
			GUILayout.EndHorizontal ();
		}

		public void OnGUI (Rect rect)
		{
			GUILayout.BeginArea (rect);
			OnGUI ();
			GUILayout.EndArea ();
		}

		public void Add (ToolbarItem item)
		{
			items.Add (item);
		}

		public ToolbarItem Get (int index)
		{
			if (index >= 0 && index < items.Count) {
				return items [index];
			}
			return null;
		}
	}

	public enum ToolbarItemPosition
	{
		None,
		Left,
		Right,
		Center
	}
}