using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Sets the value of the preference identified by key.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.SetInt.html")]
	public class SetInt: Action
	{
		public StringVariable key;
		public IntVariable value;

		public override TaskStatus OnUpdate ()
		{
			PlayerPrefs.SetInt (key.Value, value.Value);
			return TaskStatus.Success;
		}
	}
}
