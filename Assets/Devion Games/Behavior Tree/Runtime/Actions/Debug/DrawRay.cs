using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityDebug
{
	[Category ("Debug")]
	[Tooltip ("Draws a line from start to start + dir in world coordinates.")]
	[HelpURL ("http://docs.unity3d.com/Documentation/ScriptReference/Debug.DrawRay.html")]
	public class DrawRay : Action
	{
		[Tooltip ("Start position of the line.")]
		public Vector3Variable start;
		[Tooltip ("The direction of the ray.")]
		public Vector3Variable direction;
		[Tooltip ("Color of the ray.")]
		public ColorVariable color = Color.white;

		public override TaskStatus OnUpdate ()
		{
			Debug.DrawRay (start.Value, direction.Value, color.Value);
			return TaskStatus.Success;
		}
	}
}