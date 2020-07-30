using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[Tooltip ("The repeater task will repeat execution of its children until a specified number of times.")]
	[Icon ("Repeater")]
	public class Repeater : Decorator
	{
		public IntVariable m_Count;
		[Shared]
		[NotRequired]
		public IntVariable index;
		public BoolVariable repeatForever;

		private int m_CurrentCount;

		public override void OnStart ()
		{
			this.m_CurrentCount = 0;
		}

		public override TaskStatus OnUpdate ()
		{
			TaskStatus status = children [0].Tick ();

			if (status == TaskStatus.Running || repeatForever.Value) {
				return TaskStatus.Running;
			}
			++m_CurrentCount;
			index.Value = index.Value + 1;
			return m_CurrentCount < m_Count.Value ? TaskStatus.Running : status;
		}

	}
}