using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityPlayerPrefs
{
	[Category ("UnityEngine/PlayerPrefs")]
	[Tooltip ("Returns the value corresponding to key in the preference file if it exists.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/PlayerPrefs.GetFloat.html")]
	public class GetFloat: Action
	{
		public StringVariable key;
		public FloatVariable defaultValue;
		[Shared]
		public FloatVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			m_Store.Value = PlayerPrefs.GetFloat (key.Value, defaultValue.Value);
			return TaskStatus.Success;
		}
	}
}
