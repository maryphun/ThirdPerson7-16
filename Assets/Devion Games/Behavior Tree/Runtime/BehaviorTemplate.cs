using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class BehaviorTemplate : ScriptableObject, IBehavior
	{
		[SerializeField]
		private BehaviorTree m_BehaviorTree= null;

		public BehaviorTree GetBehaviorTree ()
		{
			return this.m_BehaviorTree;
		}

		public bool IsTemplate ()
		{
			return true;
		}

		public Object GetObject ()
		{
			return this;
		}
	}
}