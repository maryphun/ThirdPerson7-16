using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html")]
	public class OnCollisionEnter2D : Conditional
	{
		[NotRequired]
		[Shared]
		[Tooltip ("Stores the other game object.")]
		public GameObjectVariable otherGameObject;

		private bool m_EnteredCollision;

		public override void OnBehaviorStart ()
		{
			Events.Register<Collision2D> (gameObject, "OnCollisionEnter2D", OnCollisionEnter2DEvent);
		}

		public override void OnBehaviorComplete ()
		{
			Events.Unregister<Collision2D> (gameObject, "OnCollisionEnter2D", OnCollisionEnter2DEvent);
		}

		public override void OnStart ()
		{
			this.m_EnteredCollision = false;
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_EnteredCollision ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void OnCollisionEnter2DEvent (Collision2D other)
		{
			if (!otherGameObject.isNone) {
				otherGameObject.Value = other.gameObject;
			}
			this.m_EnteredCollision = true;
		}
	}
}