using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;

namespace DevionGames.BehaviorTrees
{
	public abstract class Task : IEnumerable, IEnabled
	{

		[SerializeField]
		private string m_Name = string.Empty;

		public string name {
			get { return this.m_Name; }
			set { this.m_Name = value; }
		}

		[HideInInspector]
		[SerializeField]
		private bool m_Enabled = true;

		public bool enabled {
			get { return this.m_Enabled; }
			set { this.m_Enabled = value; }
		}

		[SerializeField]
		private NodeInfo m_NodeInfo;

		public NodeInfo nodeInfo {
			get { 
				if (this.m_NodeInfo == null) {
					this.m_NodeInfo = new NodeInfo ();
				}
				return this.m_NodeInfo; 
			}
			set{ this.m_NodeInfo = value; }
		}

		private Task m_Parent;

		public Task parent {
			get { 
				return this.m_Parent;
			}
			set { 
				this.m_Parent = value;
			}
		}

		public virtual int maxChildCount {
			get { 
				return int.MaxValue;
			}
		}

		private GameObject m_GameObject;

		public GameObject gameObject {
			get { 
				return this.m_GameObject;
			}
		}

		private Behavior m_Owner;

		public Behavior owner {
			get { 
				return this.m_Owner;
			}
		}

		private List<Task> m_Children = new List<Task> ();

		public List<Task> children {
			get { 
				return this.m_Children;
			}
		}

		public Task this [int index] {
			get { return this.m_Children [index]; }
			set { Insert (index, value); }
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return m_Children.GetEnumerator ();
		}

		public void Add (Task child)
		{
			child.parent = this;
			this.m_Children.Add (child);
		}

		public bool Remove (Task child)
		{
			child.parent = null;
			return this.m_Children.Remove (child);
		}

		public void Insert (int index, Task child)
		{
			child.parent = this;
			this.m_Children.Insert (index, child);
		}

		public void RemoveAt (int index)
		{
			if (index > 0 && index < this.m_Children.Count) {
				this.m_Children [index].parent = null;
			}
			this.m_Children.RemoveAt (index);
		}

		public void Clear ()
		{
			for (int i = 0; i < this.m_Children.Count; i++) {
				this.m_Children [i].parent = null;
			}
			this.m_Children.Clear ();
		}

		private TaskStatus m_Status = TaskStatus.Inactive;

		public TaskStatus status {
			get { 
				return this.m_Status;
			}	
			set {
				this.m_Status = value;
			}
		}

	

		public void Initialize (Behavior owner)
		{
			this.m_Owner = owner;
			this.m_GameObject = owner.gameObject;
		}

		public TaskStatus Tick ()
		{
			#if UNITY_EDITOR
			if (nodeInfo.isBreakpoint) {
				UnityEditor.EditorApplication.isPaused = true;
				return TaskStatus.Running;
			}
			#endif
			if (!enabled) {
				return TaskStatus.Success;
			}
			if (this.m_Status != TaskStatus.Running) {
				OnStart ();
			}
			this.m_Status = OnUpdate ();

			if (this.m_Status != TaskStatus.Running) {
				OnEnd ();
			}
			return this.m_Status;
		}

		/// <summary>
		/// Called when the behavior tree started to tick.
		/// </summary>
		public virtual void OnBehaviorStart ()
		{
		}

		/// <summary>
		/// Called when the behavior tree is completed.
		/// </summary>
		public virtual void OnBehaviorComplete ()
		{
		}

		/// <summary>
		/// Called when the task is started.
		/// </summary>
		public virtual void OnStart ()
		{
		}

		/// <summary>
		/// Called when the task completed.
		/// </summary>
		public virtual void OnEnd ()
		{
	
		}

		/// <summary>
		/// Called after OnStart() and before OnEnd(). This is where you write game logic. 
		/// </summary>
		/// <returns>State of task.</returns>
		public virtual TaskStatus OnUpdate ()
		{
			return TaskStatus.Success;
		}
	}
}