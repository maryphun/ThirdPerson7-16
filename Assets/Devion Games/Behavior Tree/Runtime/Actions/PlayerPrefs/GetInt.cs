using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Returns the value corresponding to key in the preference file if it exists.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.GetInt.html")]
	public class GetInt: Action
	{

		public StringVariable key;
		public IntVariable defaultValue;
		[Shared]
		public IntVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			m_Store.Value = PlayerPrefs.GetInt (key.Value, defaultValue.Value);
			return TaskStatus.Success;
		}
	}
}
