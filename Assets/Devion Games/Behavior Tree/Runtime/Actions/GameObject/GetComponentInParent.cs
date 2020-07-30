using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Gets the component of Type type in the GameObject or any of its parents.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.GetComponentInParent.html")]
	public class GetComponentInParent : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The type of the component.")]
		public StringVariable m_Type;
		[Shared]
		[Tooltip ("The component.")]
		public ObjectVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_Store.Value = m_gameObject.Value.GetComponentInParent (TypeUtility.GetType (m_Type.Value));
			return TaskStatus.Success;
		}
	}
}