using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Writes all modified preferences to disk.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.Save.html")]
	public class Save: Action
	{

		public override TaskStatus OnUpdate ()
		{
			PlayerPrefs.Save ();
			return TaskStatus.Success;
		}
	}
}
