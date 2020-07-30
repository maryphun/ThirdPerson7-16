using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Gets the component of Type if the game object has one attached, null if it doesn't.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html")]
	public class GetComponent : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The type of the component.")]
		public StringVariable m_Type;
		[Shared]
		[Tooltip ("The component")]
		public ObjectVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_Store.Value = m_gameObject.Value.GetComponent (m_Type.Value);
			return TaskStatus.Success;
		}
	}
}