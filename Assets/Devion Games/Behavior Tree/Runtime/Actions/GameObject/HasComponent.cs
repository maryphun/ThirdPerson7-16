using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Returns success if the gameobject has the component of type.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html")]
	public class HasComponent : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The type of the component.")]
		public StringVariable m_Type;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			return this.m_gameObject.Value.GetComponent (m_Type.Value) != null ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}