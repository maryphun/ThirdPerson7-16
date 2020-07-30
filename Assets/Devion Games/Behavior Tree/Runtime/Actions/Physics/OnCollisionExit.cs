using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit.html")]
	public class OnCollisionExit : Conditional
	{
		[NotRequired]
		[Shared]
		[Tooltip ("Stores the other game object.")]
		public GameObjectVariable otherGameObject;

		private bool m_ExitedCollision;

		public override void OnBehaviorStart ()
		{
			Events.Register<Collision> (gameObject, "OnCollisionExit", OnCollisionExitEvent);
		}

		public override void OnBehaviorComplete ()
		{
			Events.Unregister<Collision> (gameObject, "OnCollisionExit", OnCollisionExitEvent);
		}

		public override void OnStart ()
		{
			this.m_ExitedCollision = false;
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_ExitedCollision ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void OnCollisionExitEvent (Collision other)
		{
			if (!otherGameObject.isNone) {
				otherGameObject.Value = other.gameObject;
			}
			this.m_ExitedCollision = true;
		}
	}
}