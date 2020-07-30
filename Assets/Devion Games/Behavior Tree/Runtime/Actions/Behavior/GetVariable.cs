using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.Variables
{
	[Category ("Behavior")]
	[Tooltip ("Gets the value of a variable.")]
	public class GetVariable: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Behavior name")]
		public StringVariable m_BehaviorName;
		[Tooltip ("Behavior name")]
		public StringVariable m_VariableName;
		[Shared]
		public GenericVariable m_Result;

		private Variable variable;

		public override void OnStart ()
		{
			variable = null;
			if (m_gameObject.Value != null) {
				Behavior[] behaviors = m_gameObject.Value.GetComponents<Behavior> ();
				for (int i = 0; i < behaviors.Length; i++) {
					BehaviorTree tree = behaviors [i].GetBehaviorTree ();
					if (tree.name == m_BehaviorName.Value && tree.blackboard.GetVariable (m_VariableName.Value) != null) {
						variable = tree.blackboard.GetVariable (m_VariableName.Value);
						break;
					}
				}
			}
		}


		public override TaskStatus OnUpdate ()
		{
			if (variable == null) {
				return TaskStatus.Failure;
			}
			m_Result.Value = variable.RawValue;
			return TaskStatus.Success;
		}

	}
}
