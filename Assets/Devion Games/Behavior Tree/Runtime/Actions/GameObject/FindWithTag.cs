using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Finds a GameObject by tag.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html")]
	public class FindWithTag : Action
	{
		[Tooltip ("The tag to search for.")]
		public StringVariable m_Tag;
		[Shared]
		[Tooltip ("Store the found game object.")]
		public GameObjectVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			m_Store.Value = GameObject.FindWithTag (m_Tag.Value);
			return TaskStatus.Success;
		}
	}
}