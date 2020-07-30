using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Is this game object tagged with tag ?")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html")]
	public class CompareTag : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The tag to compare against.")]
		public StringVariable m_Tag;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			return this.m_gameObject.Value.CompareTag (m_Tag.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}