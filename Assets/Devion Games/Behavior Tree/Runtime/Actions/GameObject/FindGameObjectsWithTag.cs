using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Gets an array of active GameObjects tagged tag.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html")]
	public class FindGameObjectsWithTag : Action
	{
		[Tooltip ("The tag to search for.")]
		public StringVariable m_Tag;
		[Tooltip ("Store the found game objects.")]
		public ArrayVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			m_Store.Value = GameObject.FindGameObjectsWithTag (m_Tag.Value);
			return TaskStatus.Success;
		}
	}
}