using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter.html")]
	public class OnCollisionEnter : Conditional
	{
		[NotRequired]
		[Shared]
		[Tooltip ("Stores the other game object.")]
		public GameObjectVariable otherGameObject;

		private bool m_EnteredCollision;

		public override void OnBehaviorStart ()
		{
			Events.Register<Collision> (gameObject, "OnCollisionEnter", OnCollisionEnterEvent);
		}

		public override void OnBehaviorComplete ()
		{
			Events.Unregister<Collision> (gameObject, "OnCollisionEnter", OnCollisionEnterEvent);
		}

		public override void OnStart ()
		{
			this.m_EnteredCollision = false;
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_EnteredCollision ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void OnCollisionEnterEvent (Collision other)
		{
			if (!otherGameObject.isNone) {
				otherGameObject.Value = other.gameObject;
			}
			this.m_EnteredCollision = true;
		}
	}
}