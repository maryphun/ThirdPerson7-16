using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Returns success if key exists in the preferences.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.HasKey.html")]
	public class HasKey: Conditional
	{
		public StringVariable key;

		public override TaskStatus OnUpdate ()
		{
			return PlayerPrefs.HasKey (key.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
