using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Finds a GameObject by name.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.Find.html")]
	public class Find : Action
	{
		[Tooltip ("The name.")]
		public StringVariable m_name;
		[Shared]
		[Tooltip ("Store the found game object.")]
		public GameObjectVariable m_Store;


		public override TaskStatus OnUpdate ()
		{
			m_Store.Value = GameObject.Find (m_name.Value);
			return TaskStatus.Success;
		}
	}
}