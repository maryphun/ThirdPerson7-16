using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class TaskError
	{
		public ErrorType type;
		public IBehavior behavior;
		public Variable variable;
		public Task node;

		public TaskError (IBehavior behavior, Task node, Variable variable, ErrorType type)
		{
			this.behavior = behavior;
			this.variable = variable;
			this.type = type;
			this.node = node;
		}

		public TaskError (IBehavior behavior, Task node, ErrorType type)
		{
			this.behavior = behavior;
			this.type = type;
			this.node = node;
		}


		public bool SameAs (TaskError error)
		{
			if (type != error.type) {
				return false;
			}
			if (behavior != error.behavior) {
				return false;
			}
			if (variable != error.variable) {
				return false;
			}
			if (node != error.node) {
				return false;
			}
			return true;
		}

		public enum ErrorType
		{
			RequiredField,
			RequiredChild
		}
	}
}