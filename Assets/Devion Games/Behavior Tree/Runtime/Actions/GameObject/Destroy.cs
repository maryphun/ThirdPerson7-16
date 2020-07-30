using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Removes a GameObject.")]
	[HelpURL ("http://docs.unity3d.com/Documentation/ScriptReference/Object.Destroy.html")]
	public class Destroy : Action
	{
		[Tooltip ("The game object to destroy.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The optional amount of time to delay before destroying the game object.")]
		public FloatVariable delay;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			if (delay.Value == 0f) {
				GameObject.Destroy (this.m_gameObject.Value);
			} else {
				GameObject.Destroy (this.m_gameObject.Value, delay.Value);
			}
			return TaskStatus.Success;
		}
	}
}