using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityDebug
{
	[Category ("Debug")]
	[Tooltip ("Prints a message to the console.")]
	[HelpURL ("http://docs.unity3d.com/Documentation/ScriptReference/Debug.Log.html")]
	public class Log : Action
	{
		[Tooltip ("Message to print in the console.")]
		public GenericVariable argument;
		[Tooltip ("Log type.")]
		public LogType type;

		public override TaskStatus OnUpdate ()
		{
			switch (type) {
			case LogType.Normal:
				Debug.Log (argument.Value);
				break;
			case LogType.Warning:
				Debug.LogWarning (argument.Value);
				break;
			case LogType.Error:
				Debug.LogError (argument.Value);
				break;
			}
			return TaskStatus.Success;
		}

		public enum LogType
		{
			Normal,
			Warning,
			Error
		}
	}
}
