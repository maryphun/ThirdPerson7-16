using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("The local active state of this GameObject.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject-activeSelf.html")]
	public class ActiveSelf : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			return this.m_gameObject.Value.activeSelf ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}