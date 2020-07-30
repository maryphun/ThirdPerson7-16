using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[Tooltip ("Similar to the sequence taks, the parallel task will run all the children tasks at once. You can use this composite node to run more than one taks at a time.")]
	[Icon ("Parallel")]
	public class Parallel : Composite
	{
		private TaskStatus[] childrenStatus;

		public override void OnStart ()
		{
			childrenStatus = new TaskStatus[children.Count];
			for (int i = 0; i < childrenStatus.Length; i++) {
				childrenStatus [i] = TaskStatus.Running;
			}
		}

		public override TaskStatus OnUpdate ()
		{
			bool completed = true;
			for (int i = 0; i < childrenStatus.Length; i++) {
				if (childrenStatus [i] == TaskStatus.Running) {
					TaskStatus childStatus = children [i].Tick ();
					childrenStatus [i] = childStatus;
					if (childStatus == TaskStatus.Running) {
						completed = false;
					}
				}


			}
			
			return completed ? TaskStatus.Success : TaskStatus.Running;
		}
	}
}