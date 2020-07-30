using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	[CustomObjectDrawer (typeof(NodeInfo))]
	public class NodeInfoDrawer : ObjectDrawer
	{
		public override void OnGUI (GUIContent label)
		{
			NodeInfo info = value as NodeInfo;
			EditorGUILayout.LabelField ("Comment");
			info.comment = EditorGUILayout.TextArea (info.comment, EditorStyles.textArea, GUILayout.Height (50f));
		}
	}
}