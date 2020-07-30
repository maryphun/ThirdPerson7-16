using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Removes all keys and values from the preferences. Use with caution.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.DeleteAll.html")]
	public class DeleteAll: Action
	{

		public override TaskStatus OnUpdate ()
		{
			PlayerPrefs.DeleteAll ();
			return TaskStatus.Success;
		}
	}
}
