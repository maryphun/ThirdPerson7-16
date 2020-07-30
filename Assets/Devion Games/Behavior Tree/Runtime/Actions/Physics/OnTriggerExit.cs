using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPhysics
{
	[Category ("UnityEngine/Physics")]
	[Tooltip ("OnTriggerExit is called when the game object other has stopped touching the trigger.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerExit.html")]
	public class OnTriggerExit : Conditional
	{
		[NotRequired]
		[Shared]
		[Tooltip ("Stores the other game object.")]
		public GameObjectVariable otherGameObject;

		private bool m_ExitedTrigger;

		public override void OnBehaviorStart ()
		{
			Events.Register<Collider> (gameObject, "OnTriggerExit", OnTriggerExitEvent);
		}

		public override void OnBehaviorComplete ()
		{
			Events.Unregister<Collider> (gameObject, "OnTriggerExit", OnTriggerExitEvent);
		}

		public override void OnStart ()
		{
			this.m_ExitedTrigger = false;
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_ExitedTrigger ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void OnTriggerExitEvent (Collider other)
		{
			if (!otherGameObject.isNone) {
				otherGameObject.Value = other.gameObject;
			}
			this.m_ExitedTrigger = true;
		}
	}
}