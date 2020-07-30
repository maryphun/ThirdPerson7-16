using System.Collections;
using System.Collections.Generic;
using DevionGames.BehaviorTrees.Conditionals;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[Tooltip ("\"OR\" logic. It will process the first child, and if it fails will process the second, and if that fails will process the third, until a success is reached, at which point it will instantly return success. It will fail if all children fail.")]
	[Icon ("Selector")]
	public class Selector : Composite
	{
		private int m_CurrentChildIndex = 0;

		public override void OnStart ()
		{
			this.m_CurrentChildIndex = 0;
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_CurrentChildIndex >= children.Count) {
				m_CurrentChildIndex = 0;
			}

			TaskStatus childStatus = TaskStatus.Inactive;

			while (m_CurrentChildIndex < children.Count) {
				if (CheckAbortSelf() || CheckAbortPriority())
				{
					List<Task> tasks = new List<Task>();
					BehaviorUtility.GetNodesRecursive(children[m_CurrentChildIndex], ref tasks);
					for (int i = 0; i < tasks.Count; i++)
					{
						if (tasks[i].status == TaskStatus.Running)
						{
							tasks[i].OnEnd();
							tasks[i].status = TaskStatus.Inactive;
						}
					}
					break;
				}

				childStatus = children [m_CurrentChildIndex].Tick ();
				if (childStatus == TaskStatus.Failure) {
					++m_CurrentChildIndex;
				} else {
					break;
				}
			}
			return childStatus;
		}


		public bool CheckAbortSelf()
		{

			if ((abortType == AbortType.Self || abortType == AbortType.Both) && status == TaskStatus.Running)
			{
				for (int i = 0; i < children.Count; i++)
				{
					Conditional conditional = children[i] as Conditional;

					if (conditional != null && i < m_CurrentChildIndex)
					{
						TaskStatus childStatus = children[i].Tick();
						if (childStatus == TaskStatus.Success)
						{
							//Debug.Log("Self Abort: " + children[i].name);
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool CheckAbortPriority()
		{

			if (status == TaskStatus.Running)
			{

				for (int i = 0; i < children.Count; i++)
				{
					Composite composite = children[i] as Composite;
					if (composite != null && (composite.abortType == AbortType.LowerPriority || composite.abortType == AbortType.Both))
					{

						for (int j = 0; j < composite.children.Count; j++)
						{
							Conditional conditional = composite.children[j] as Conditional;

							if (conditional != null)
							{

								TaskStatus childStatus = composite.children[j].Tick();
								if (childStatus == TaskStatus.Failure)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

	
	}
}