using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityTime
{
	[Category ("UnityEngine/Time")]
	[Tooltip ("Wait a specified amount of time in seconds. The action will return running until the action is done waiting. It will return success after the wait time has elapsed.")]
	[Icon ("Wait")]
	public class Wait : Action
	{
		public FloatVariable waitTime = 1f;
		public BoolVariable randomWait;
		public FloatVariable minWaitTime;
		public FloatVariable maxWaitTime = 1f;

		private float time = 0f;
		private float duration;

		public override void OnStart ()
		{
			time = 0f;
			if (randomWait.Value) {
				duration = Random.Range (minWaitTime.Value, maxWaitTime.Value);
			} else {
				duration = waitTime.Value;
			}
		}

		public override TaskStatus OnUpdate ()
		{
			time += Time.deltaTime;
			if (time > duration) {
				return TaskStatus.Success;
			}

			return TaskStatus.Running;
		}

        public override void OnEnd()
        {
			Debug.Log("End Wait");
        }
    }
}