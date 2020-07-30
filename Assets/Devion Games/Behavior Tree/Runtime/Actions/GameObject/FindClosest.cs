using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Finds the closest GameObject by name.")]
	public class FindClosest : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The name.")]
		public StringVariable m_name;
		[Shared]
		[Tooltip ("Store the found game object.")]
		public GameObjectVariable m_Store;


		public override TaskStatus OnUpdate ()
		{
			if (m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_Store.Value = FindClosestByName (m_gameObject.Value, m_name.Value);
			return TaskStatus.Success;
		}

		private GameObject FindClosestByName (GameObject target, string name)
		{
			Transform[] transforms = GameObject.FindObjectsOfType<Transform> ();
			List<GameObject> gos = transforms.Select (x => x.gameObject).Where (y => y.name == name).ToList ();

			GameObject closest = null; 
			float distance = Mathf.Infinity; 
			Vector3 position = target.transform.position; 
			foreach (GameObject go in gos) { 
				Vector3 diff = (go.transform.position - position);
				float curDistance = diff.sqrMagnitude; 
				if (curDistance < distance && go.transform != target.transform) { 
					closest = go; 
					distance = curDistance; 
				} 
			} 
			return closest;
		}
	}
}