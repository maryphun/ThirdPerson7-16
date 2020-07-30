using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityPhysics2D
{
	[Category ("UnityEngine/Physics2D")]
	[Tooltip ("OnTriggerEnter2D is called when the game object other enters the trigger.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html")]
	public class OnTriggerEnter2D : Conditional
	{
		[NotRequired]
		[Shared]
		[Tooltip ("Stores the other game object.")]
		public GameObjectVariable otherGameObject;

		private bool m_EnteredTrigger;

		public override void OnBehaviorStart ()
		{
			Events.Register<Collider> (gameObject, "OnTriggerEnter2D", OnTriggerEnter2DEvent);
		}

		public override void OnBehaviorComplete ()
		{
			Events.Unregister<Collider> (gameObject, "OnTriggerEnter2D", OnTriggerEnter2DEvent);
		}

		public override void OnStart ()
		{
			this.m_EnteredTrigger = false;
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_EnteredTrigger ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void OnTriggerEnter2DEvent (Collider other)
		{
			if (!otherGameObject.isNone) {
				otherGameObject.Value = other.gameObject;
			}
			this.m_EnteredTrigger = true;
		}
	}
}