using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions
{
	[Category ("Behavior")]
	[Tooltip ("Disables the behavior by name.")]
	public class DisableBehavior : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Behavior name")]
		public StringVariable m_name;
		[Tooltip ("Pause the behavior?")]
		public BoolVariable m_Pause;

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
			if (behavior == null) {
				return TaskStatus.Failure;
			}
			behavior.DisableBehavior (m_Pause.Value);
			return TaskStatus.Success;
		}

	}
}
