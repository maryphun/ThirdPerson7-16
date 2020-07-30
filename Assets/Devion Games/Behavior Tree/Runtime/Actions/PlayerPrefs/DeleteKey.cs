using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Removes key and its corresponding value from the preferences.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.DeleteKey.html")]
	public class DeleteKey: Action
	{
		public StringVariable key;

		public override TaskStatus OnUpdate ()
		{
			PlayerPrefs.DeleteKey (key.Value);
			return TaskStatus.Success;
		}
	}
}
