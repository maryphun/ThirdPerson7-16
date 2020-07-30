using UnityEngine;
using System.Collections;

namespace DevionGames.BehaviorTrees.Actions.UnityDebug
{
	[Category ("Debug")]
	[Tooltip ("Draws a line between specified start and end points.")]
	[HelpURL ("http://docs.unity3d.com/Documentation/ScriptReference/Debug.DrawLine.html")]
	public class DrawLine : Action
	{
		[Tooltip ("Start position of the line.")]
		public Vector3Variable start;
		[Tooltip ("End position of the line.")]
		public Vector3Variable end;
		[Tooltip ("Color of the line.")]
		public ColorVariable color = Color.white;
		[Tooltip ("How long the line should be visible for.")]
		public FloatVariable duration = 1f;
		[Tooltip ("Should the line be obscured by objects closer to the camera?")]
		public BoolVariable dephTest = true;

		public override TaskStatus OnUpdate ()
		{
			Debug.DrawLine (start.Value, end.Value, color.Value, duration.Value, dephTest.Value);
			return TaskStatus.Success;
		}

	}
}