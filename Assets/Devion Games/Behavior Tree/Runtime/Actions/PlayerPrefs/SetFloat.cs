using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Sets the value of the preference identified by key.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.SetFloat.html")]
	public class SetFloat: Action
	{
		public StringVariable key;
		public FloatVariable value;

		public override TaskStatus OnUpdate ()
		{
			PlayerPrefs.SetFloat (key.Value, value.Value);
			return TaskStatus.Success;
		}
	}
}
