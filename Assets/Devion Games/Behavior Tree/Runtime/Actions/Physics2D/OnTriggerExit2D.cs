using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("OnTriggerExit2D is called when the game object other has stopped touching the trigger.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerExit2D.html")]
	public class OnTriggerExit2D : Conditional
	{
		[NotRequired]
		[Shared]
		[Tooltip ("Stores the other game object.")]
		public GameObjectVariable otherGameObject;

		private bool m_ExitedTrigger;

		public override void OnBehaviorStart ()
		{
			Events.Register<Collider> (gameObject, "OnTriggerExit2D", OnTriggerExit2DEvent);
		}

		public override void OnBehaviorComplete ()
		{
			Events.Unregister<Collider> (gameObject, "OnTriggerExit2D", OnTriggerExit2DEvent);
		}

		public override void OnStart ()
		{
			this.m_ExitedTrigger = false;
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_ExitedTrigger ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void OnTriggerExit2DEvent (Collider other)
		{
			if (!otherGameObject.isNone) {
				otherGameObject.Value = other.gameObject;
			}
			this.m_ExitedTrigger = true;
		}
	}
}