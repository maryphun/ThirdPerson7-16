using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Gets all components of Type type in the GameObject.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.GetComponents.html")]
	public class GetComponents : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The type of the components.")]
		public StringVariable m_Type;
		[Tooltip ("The components.")]
		public ArrayVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_Store.Value = m_gameObject.Value.GetComponents (TypeUtility.GetType (m_Type.Value));
			return TaskStatus.Success;
		}
	}
}