using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevionGames.BehaviorTrees.Conditionals;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	public class Behavior : MonoBehaviour, IBehavior
	{
		[SerializeField]
		private BehaviorTree m_BehaviorTree;
		[HideInInspector]
		[SerializeField]
		public BehaviorTemplate m_Template;
		[SerializeField]
		private TickEvent m_TickEvent = TickEvent.Update;
		public bool startWhenEnabled = true;
		public bool restartWhenComplete = false;
		public bool checkConfiguration = true;
		public bool showSceneIcon = true;

		private bool m_Paused;
		private bool m_Initialized;
		private TaskStatus status = TaskStatus.Inactive;
		

        private void OnEnable ()
		{
			if (startWhenEnabled) {
				EnableBehavior ();
			}
		}

		private void Update ()
		{
			if (m_TickEvent == TickEvent.Update)
				Tick();
		}


		private void LateUpdate()
		{
			if (m_TickEvent == TickEvent.LateUpdate)
				Tick();
		}


		private void FixedUpdate()
		{
			if (m_TickEvent == TickEvent.FixedUpdate)
				Tick();
		}

		private void Tick() {
			if (status == TaskStatus.Running && !m_Paused && this.m_Initialized)
			{
				status = this.m_BehaviorTree.root.Tick();
				if (status != TaskStatus.Running)
				{
					Task[] activeTasks = BehaviorUtility.GetAllActiveNodes(this.m_BehaviorTree);
					for (int i = 0; i < activeTasks.Length; i++)
					{
						Task task = activeTasks[i];
						task.OnBehaviorComplete();
					}
					if (restartWhenComplete)
					{
						RestartBehavior();
					}
				}
			}
		}

		public void EnableBehavior ()
		{
			if (this.m_Template != null) {
				this.m_Template = (BehaviorTemplate)Instantiate (this.m_Template);
			}
			this.m_BehaviorTree = GetBehaviorTree ();
			if (this.m_Paused) {
				this.m_Paused = false;
				return;
			}
			enabled = true;
			if (this.m_Initialized) {
				RestartBehavior ();
				return;
			}
			if (!this.Initialize ()) {
				Debug.LogWarning ("Behavior Tree: \"" + this.m_BehaviorTree.name + "\" on Game Object: \"" + gameObject.name + "\" has task configuration errors and was disabled!");
				status = TaskStatus.Inactive;
			} else {
				this.m_Initialized = true;
				status = TaskStatus.Running;
			}
		}

		public void DisableBehavior (bool pause)
		{
			this.m_Paused = pause;
			if (!pause) {
				enabled = false;
			}
		}

		public void RestartBehavior ()
		{
			StartCoroutine (Restart ());
		}

		private IEnumerator Restart ()
		{
			yield return new WaitForEndOfFrame ();
			this.m_Paused = false;
			status = TaskStatus.Inactive;
			Task[] activeTasks = BehaviorUtility.GetAllActiveNodes (this.m_BehaviorTree);
			for (int i = 0; i < activeTasks.Length; i++) {
				Task task = activeTasks [i];
				task.OnBehaviorComplete ();
			}

			for (int i = 0; i < activeTasks.Length; i++) {
				Task task = activeTasks [i];
				task.status = TaskStatus.Inactive;
				task.OnBehaviorStart ();
			}
			status = TaskStatus.Running;
		}


		private bool Initialize ()
		{
			if (checkConfiguration && BehaviorUtility.HasConfigurationErrors (this)) {
				return false;
			}
			this.m_BehaviorTree.blackboard.GetVariable ("Self", false).RawValue = gameObject;
			this.m_BehaviorTree.UpdateReferencedVariables ();

			Task[] activeTasks = BehaviorUtility.GetAllActiveNodes (this.m_BehaviorTree);
			for (int i = 0; i < activeTasks.Length; i++) {
				Task task = activeTasks [i];
				task.Initialize (this);
				task.OnBehaviorStart ();
			}
			return true;
		}

		public BehaviorTree GetBehaviorTree ()
		{
			if (this.m_BehaviorTree == null) {
				this.m_BehaviorTree = new BehaviorTree ();
			}
			return this.m_Template != null ? this.m_Template.GetBehaviorTree () : this.m_BehaviorTree;
		}

		public bool IsTemplate ()
		{
			return this.m_Template != null;
		}

		public Object GetObject ()
		{
			return gameObject;
		}

		public Variable GetVariable (string name)
		{
			return GetBehaviorTree ().blackboard.GetVariable (name, true);
		}

		public T GetVariableValue<T> (string name)
		{
			Variable variable = GetBehaviorTree ().blackboard.GetVariable (name, true);
			if (variable != null) {
				return (T)variable.RawValue;
			}
			return default(T);
		}

		public object GetVariableValue (string name)
		{
			Variable variable = GetBehaviorTree ().blackboard.GetVariable (name, true);
			if (variable != null) {
				return variable.RawValue;
			}
			return null;
		}

		public void SetVariableValue (string name, object value)
		{
			Variable variable = GetBehaviorTree ().blackboard.GetVariable (name, true);
			if (variable != null) {
				variable.RawValue = value;
			}
		}

		private void OnTriggerEnter (Collider other)
		{
			Events.Invoke<Collider> (gameObject, "OnTriggerEnter", other);
		}

		private void OnTriggerExit (Collider other)
		{
			Events.Invoke<Collider> (gameObject, "OnTriggerExit", other);
		}

		private void OnTriggerEnter2D (Collider2D other)
		{
			Events.Invoke<Collider2D> (gameObject, "OnTriggerEnter2D", other);
		}

		private void OnTriggerExit2D (Collider2D other)
		{
			Events.Invoke<Collider2D> (gameObject, "OnTriggerExit2D", other);
		}

		private void OnCollisionEnter (Collision collision)
		{
			Events.Invoke<Collision> (gameObject, "OnCollisionEnter", collision);
		}

		private void OnCollisionExit (Collision collision)
		{
			Events.Invoke<Collision> (gameObject, "OnCollisionExit", collision);
		}

		private void OnCollisionEnter2D (Collision2D collision)
		{
			Events.Invoke<Collision2D> (gameObject, "OnCollisionEnter2D", collision);
		}

		private void OnCollisionExit2D (Collision2D collision)
		{
			Events.Invoke<Collision2D> (gameObject, "OnCollisionExit2D", collision);
		}

		public enum TickEvent { 
			Update, 
			LateUpdate,
			FixedUpdate
		}
	}
}