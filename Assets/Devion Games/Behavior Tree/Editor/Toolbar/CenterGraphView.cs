using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class CenterGraphView : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (new GUIContent (Styles.skin.FindStyle ("Center").normal.background, "Center Graph View"), EditorStyles.toolbarButton, GUILayout.Width (25f))) {
				BehaviorTreeWindow.current.m_Graph.CenterGraphView ();
			}
		}
	}
}