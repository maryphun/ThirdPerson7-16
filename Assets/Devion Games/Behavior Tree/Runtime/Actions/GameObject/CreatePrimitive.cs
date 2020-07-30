using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Creates a game object with a primitive mesh renderer and appropriate collider.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html")]
	public class CreatePrimitive : Action
	{
		[Tooltip ("The type of primitive object to create.")]
		public PrimitiveType m_type;
		[Shared]
		[NotRequired]
		[Tooltip ("The primitive.")]
		public GameObjectVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			m_Store.Value = GameObject.CreatePrimitive (m_type);
			return TaskStatus.Success;
		}
	}
}