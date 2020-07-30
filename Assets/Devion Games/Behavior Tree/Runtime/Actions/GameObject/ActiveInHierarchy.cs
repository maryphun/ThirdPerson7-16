using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Is the GameObject active in the scene?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject-activeInHierarchy.html")]
	public class ActiveInHierarchy : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			return this.m_gameObject.Value.activeInHierarchy ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}