using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class NodeInfo
	{
		[SerializeField]
		private int m_ID = -1;

		public int id {
			get { return this.m_ID; }
			set { this.m_ID = value; }
		}

		[SerializeField]
		private Vector2 m_Position;

		public Vector2 position {
			get { return this.m_Position; }
			set{ this.m_Position = value; }
		}

		[SerializeField]
		private bool m_IsBreakpoint = false;

		public bool isBreakpoint {
			get{ return this.m_IsBreakpoint; }
			set{ this.m_IsBreakpoint = value; }
		}

		[SerializeField]
		private bool m_IsCollapsed = false;

		public bool isCollapsed {
			get{ return this.m_IsCollapsed; }
			set { 
				this.m_IsCollapsed = value;
			}
		}

		[SerializeField]
		private string m_Comment = string.Empty;

		public string comment {
			get{ return this.m_Comment; }
			set{ this.m_Comment = value; }
		}


	}
}