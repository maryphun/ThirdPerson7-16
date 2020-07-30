using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[Tooltip ("Similar to the repeater task, the until taks will repeat exuecution until the child returns a specified TaskStatus.")]
	[Icon ("Until")]
	public class Until : Decorator
	{
		public TaskStatus m_UntilStatus = TaskStatus.Success;

		public override TaskStatus OnUpdate ()
		{
			TaskStatus childStatus = children [0].Tick ();
			if (m_UntilStatus != childStatus) {
				children [0].status = TaskStatus.Running;
				return TaskStatus.Running;
			}

			return childStatus;
		}
	}
}