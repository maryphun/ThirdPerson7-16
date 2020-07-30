using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Gets the component of Type type in the GameObject or any of its children using depth first search.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.GetComponentInChildren.html")]
	public class GetComponentInChildren : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The type of the component.")]
		public StringVariable m_Type;
		[NotRequired]
		[Tooltip ("Include inactive components?")]
		public BoolVariable m_IncludeInactive;
		[Shared]
		[Tooltip ("The component")]
		public ObjectVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}

			m_Store.Value = m_gameObject.Value.GetComponentInChildren (TypeUtility.GetType (m_Type.Value), m_IncludeInactive.Value);
			return TaskStatus.Success;
		}
	}
}