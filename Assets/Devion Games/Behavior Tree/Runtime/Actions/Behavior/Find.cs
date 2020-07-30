using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions
{
	[Category ("Behavior")]
	[Tooltip ("Finds the behavior by name.")]
	public class Find : Action
	{
		[Tooltip ("Behavior name")]
		public StringVariable m_name;
		[Shared]
		public GameObjectVariable m_StoreGameObject;

		public override TaskStatus OnUpdate ()
		{
			Behavior[] behaviors = GameObject.FindObjectsOfType<Behavior> ().Where (x => x.GetBehaviorTree ().name == m_name.Value).ToArray ();
			if (behaviors.Length > 0) {
				m_StoreGameObject.Value = behaviors [Random.Range (0, behaviors.Length)].gameObject;
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}

	}
}
