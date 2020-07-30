using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Activates/Deactivates the GameObject.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html")]
	public class SetActive : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Activate or deactivation the object.")]
		public BoolVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_gameObject.Value.SetActive (m_value.Value);
			return TaskStatus.Success;
		}
	}
}