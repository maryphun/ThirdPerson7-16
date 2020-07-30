using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions
{
	[Category ("Behavior")]
	[Tooltip ("Finds the closest behavior by name.")]
	public class FindClosest : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Behavior name")]
		public StringVariable m_name;
		[Shared]
		public GameObjectVariable m_StoreGameObject;

		public override TaskStatus OnUpdate ()
		{
			m_StoreGameObject.Value = FindClosestByName (m_gameObject.Value, m_name.Value);
			return TaskStatus.Success;
		}

		private GameObject FindClosestByName (GameObject target, string name)
		{
			Behavior[] behaviors = GameObject.FindObjectsOfType<Behavior> ();
			List<GameObject> gos = behaviors.Select (x => x.gameObject).Where (y => y.name == name).ToList ();

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
