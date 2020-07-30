using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityInput
{
	[Category ("UnityEngine/Input")]
	[Tooltip ("Gets information under the mouse.")]
	public class MousePick: Action
	{
		[Tooltip ("The length of the ray.")]
		public FloatVariable distance = -1;
		[Tooltip ("Layer masks can be used selectively filter game objects for example when casting rays.")]
		public LayerMask layerMask;

		[Shared]
		[NotRequired]
		[Tooltip ("The distance from the ray's origin to the impact point.")]
		public FloatVariable hitDistance;
		[Shared]
		[NotRequired]
		[Tooltip ("The normal of the surface the ray hit.")]
		public Vector3Variable hitNormal;
		[Shared]
		[NotRequired]
		[Tooltip ("The impact point in world space where the ray hit the collider.")]
		public Vector3Variable hitPoint;
		[Shared]
		[NotRequired]
		[Tooltip ("The GameObject of the rigidbody or collider that was hit.")]
		public GameObjectVariable hitGameObject;

		public override TaskStatus OnUpdate ()
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray.origin, ray.direction, out hit, (distance.isNone || distance.Value == -1f ? Mathf.Infinity : distance.Value), layerMask)) {
				if (!hitDistance.isNone)
				{
					Debug.Log(hitDistance.isNone);
					hitDistance.Value = hit.distance;
				}
				if (!hitNormal.isNone)
					hitNormal.Value = hit.normal;
				if (!hitPoint.isNone)
					hitPoint.Value = hit.point;
				if (!hitGameObject.isNone)
					hitGameObject.Value = hit.transform.gameObject;

				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}
	}
}
