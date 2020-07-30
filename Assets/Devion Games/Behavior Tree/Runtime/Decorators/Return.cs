using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[Tooltip ("Returns the specified TaskStatus.")]
	[Icon ("Until")]
	public class Return : Decorator
	{
		public TaskStatus m_status = TaskStatus.Success;

		public override int maxChildCount {
			get {
				return 0;
			}
		}

		public override TaskStatus OnUpdate ()
		{
			return m_status;
		}
	}
}