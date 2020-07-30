using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class AlignHorizontalField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (new GUIContent (Styles.skin.FindStyle ("Horizontal Center").normal.background, "Align tasks on horizontal axis"), EditorStyles.toolbarButton, GUILayout.Width (25f))) {
				BehaviorTreeWindow.current.m_Graph.AlignNodesHorizontal ();
			}
		}
	}
}