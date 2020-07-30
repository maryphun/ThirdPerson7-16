using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("OnCollisionExit2D is called when this collider/rigidbody has stopped touching another rigidbody/collider.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit2D.html")]
	public class OnCollision2DExit : Conditional
	{
		[NotRequired]
		[Shared]
		[Tooltip ("Stores the other game object.")]
		public GameObjectVariable otherGameObject;

		private bool m_ExitedCollision;

		public override void OnBehaviorStart ()
		{
			Events.Register<Collision2D> (gameObject, "OnCollisionExit2D", OnCollisionExit2DEvent);
		}

		public override void OnBehaviorComplete ()
		{
			Events.Unregister<Collision2D> (gameObject, "OnCollisionExit2D", OnCollisionExit2DEvent);
		}

		public override void OnStart ()
		{
			this.m_ExitedCollision = false;
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_ExitedCollision ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void OnCollisionExit2DEvent (Collision2D other)
		{
			if (!otherGameObject.isNone) {
				otherGameObject.Value = other.gameObject;
			}
			this.m_ExitedCollision = true;
		}
	}
}