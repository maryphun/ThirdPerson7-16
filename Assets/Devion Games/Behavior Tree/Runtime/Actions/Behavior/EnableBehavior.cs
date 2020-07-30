using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions
{
	[Category ("Behavior")]
	[Tooltip ("Enables the behavior by name.")]
	public class EnableBehavior : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Behavior name")]
		public StringVariable m_name;

		private DevionGames.BehaviorTrees.Behavior behavior;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null) {
				Behavior[] behaviors = m_gameObject.Value.GetComponents<Behavior> ();
				for (int i = 0; i < behaviors.Length; i++) {
					BehaviorTree tree = behaviors [i].GetBehaviorTree ();
					if (tree.name == m_name.Value) {
						behavior = behaviors [i];
						break;
					}
				}
			}
		}


		public override TaskStatus OnUpdate ()
		{
			if (behavior == null) {
				return TaskStatus.Failure;
			}
			behavior.EnableBehavior ();
			return TaskStatus.Success;
		}

	}
}
