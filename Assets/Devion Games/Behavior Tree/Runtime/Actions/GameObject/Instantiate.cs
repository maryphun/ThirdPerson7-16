using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Instantiates a new GameObject.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Object.Instantiate.html")]
	public class Instantiate : Action
	{
		[Tooltip ("The game object to instantiate.")]
		public GameObjectVariable m_Original;
		[Tooltip ("The position of the new game object.")]
		public Vector3Variable m_Position;
		[Tooltip ("The rotation of the new game object.")]
		public Vector3Variable m_Rotation;
		[Tooltip ("Store the new game object.")]
		[Shared]
		[NotRequired]
		public GameObjectVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_Original.Value == null) {
				return TaskStatus.Failure;
			}
			m_Store.Value = GameObject.Instantiate (this.m_Original.Value, m_Position.Value, Quaternion.Euler (m_Rotation.Value));
			return TaskStatus.Success;
		}
	}
}