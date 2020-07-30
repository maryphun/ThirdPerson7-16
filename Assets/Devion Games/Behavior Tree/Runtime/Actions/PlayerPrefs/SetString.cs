using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Sets the value of the preference identified by key.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.SetString.html")]
	public class SetString: Action
	{
		public StringVariable key;
		public StringVariable value;

		public override TaskStatus OnUpdate ()
		{
			PlayerPrefs.SetString (key.Value, value.Value);
			return TaskStatus.Success;
		}
	}
}
