using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class BehaviorTree:ISerializationCallbackReceiver
	{
		[HideInInspector]
		[TextArea (30, 50)]
		public string serializationData;
		[HideInInspector]
		public List<Object> serializedObjects = new List<Object> ();
		public List<VariableReference> variables = new List<VariableReference> ();
		public bool serialize = false;

		[SerializeField]
		private string m_Name = "Behavior Tree";

		public string name {
			get{ return this.m_Name; }
			set{ this.m_Name = value; }
		}


		public void OnBeforeSerialize ()
		{
			//if (serialize) {
			//	Debug.Log ("Serialize");
			//BehaviorUtility.Save (this);
			//	serialize = false;
			//}
		}


		public void OnAfterDeserialize ()
		{
			//Debug.Log ("OnAfterDeserialize " + name);
			BehaviorUtility.Load (this);
		}

		/*public bool startWhenEnabled = true;
		public bool restartWhenComplete = false;
		public bool checkConfiguration = true;
		public bool showSceneIcon = true;*/
		[TextArea]
		public string description;

		//[HideInInspector]
		[SerializeField]
		private Blackboard m_Blackboard;

		public Blackboard blackboard {
			get { 
				if (this.m_Blackboard == null) {
					this.m_Blackboard = new Blackboard ();
				}
				return this.m_Blackboard;
			}
			set { 
				this.m_Blackboard = value;
			}
		}


		private List<Task> m_DetachedNodes;

		public List<Task> detachedNodes {
			get {
				if (this.m_DetachedNodes == null) {
					this.m_DetachedNodes = new List<Task> ();
				}
				return this.m_DetachedNodes;
			}

		}

		private Task m_Root;

		public Task root {
			get{ return this.m_Root; }
			set{ this.m_Root = value; }
		}

		public void UpdateReferencedVariables ()
		{
			for (int i = 0; i < variables.Count; i++) {
				VariableReference reference = variables [i];

				reference.Update (this);
			}
		}
	}
}
