using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals
{
	[Category ("Behavior")]
	[Tooltip ("Returns success if the game object has the behavior with name.")]
	public class HasBehavior : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Behavior name")]
		public StringVariable m_name;

		private Behavior behavior;

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
			if (behavior != null) {
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

	}
}
