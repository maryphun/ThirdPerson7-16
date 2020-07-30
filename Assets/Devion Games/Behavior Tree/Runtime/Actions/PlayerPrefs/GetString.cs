using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Returns the value corresponding to key in the preference file if it exists.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.GetString.html")]
	public class GetString: Action
	{
		public StringVariable key;
		public StringVariable defaultValue;
		[Shared]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			m_Store.Value = PlayerPrefs.GetString (key.Value, defaultValue.Value);
			return TaskStatus.Success;
		}
	}
}
