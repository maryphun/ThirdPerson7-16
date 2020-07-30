using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[Tooltip ("The inverter task will invert the return value of the child task after it has finished executing. If the child returns success, the inverter task will return failure. If the child returns failure, the inverter task will return success.")]
	[Icon ("Inverter")]
	public class Inverter : Decorator
	{
		public override TaskStatus OnUpdate ()
		{
			TaskStatus status = children [0].Tick ();
			if (status == TaskStatus.Success) {
				return TaskStatus.Failure;
			} else if (status == TaskStatus.Failure) {
				return TaskStatus.Success;
			}
			return status;
		}
	}
}