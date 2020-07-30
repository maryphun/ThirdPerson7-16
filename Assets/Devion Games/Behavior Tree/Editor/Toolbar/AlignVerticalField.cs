using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	public class AlignVerticalField : ToolbarItem
	{
		public override void OnGUI ()
		{
			if (GUILayout.Button (new GUIContent (Styles.skin.FindStyle ("Vertical Center").normal.background, "Align tasks on vertical axis"), EditorStyles.toolbarButton, GUILayout.Width (25f))) {
				BehaviorTreeWindow.current.m_Graph.AlignNodesVertical ();
			}
		}
	}
}