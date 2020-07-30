using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals
{
	[Category ("Behavior")]
	[Tooltip ("Returns success when the event has been received.")]
	public class OnReceiveEvent : Conditional
	{
		[Tooltip ("Event name")]
		public StringVariable m_EventName;

		[Shared]
		[NotRequired]
		[Tooltip ("Optional Argument")]
		public GenericVariable m_Argument1;
		[NotRequired]
		[Shared]
		[Tooltip ("Optional Argument")]
		public GenericVariable m_Argument2;
		[NotRequired]
		[Shared]
		[Tooltip ("Optional Argument")]
		public GenericVariable m_Argument3;

		private bool m_Received;

		public override void OnBehaviorStart ()
		{
			int count = GetArgumentsCount ();
			switch (count) {
			case 0:
				Events.Register (gameObject, m_EventName.Value, EventReceived);
				Events.Register (m_EventName.Value, EventReceived);
				break;
			case 1:
				Events.Register<object> (gameObject, m_EventName.Value, EventReceived);
				Events.Register<object> (m_EventName.Value, EventReceived);
				break;
			case 2:
				Events.Register<object,object> (gameObject, m_EventName.Value, EventReceived);
				Events.Register<object,object> (m_EventName.Value, EventReceived);
				break;
			case 3:
				Events.Register<object,object,object> (gameObject, m_EventName.Value, EventReceived);
				Events.Register<object,object,object> (m_EventName.Value, EventReceived);
				break;
			}


		}

		public override void OnBehaviorComplete ()
		{
			int count = GetArgumentsCount ();
			switch (count) {
			case 0:
				Events.Unregister (gameObject, m_EventName.Value, EventReceived);	
				Events.Unregister (m_EventName.Value, EventReceived);
				break;
			case 1:
				Events.Unregister<object> (gameObject, m_EventName.Value, EventReceived);
				Events.Unregister<object> (m_EventName.Value, EventReceived);
				break;
			case 2:
				Events.Unregister<object,object> (gameObject, m_EventName.Value, EventReceived);
				Events.Unregister<object,object> (m_EventName.Value, EventReceived);
				break;
			case 3:
				Events.Unregister<object,object,object> (gameObject, m_EventName.Value, EventReceived);
				Events.Unregister<object,object,object> (m_EventName.Value, EventReceived);
				break;
			}
		}

		public override void OnStart ()
		{
			this.m_Received = false;
		}

		public override TaskStatus OnUpdate ()
		{

			return this.m_Received ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void EventReceived ()
		{
			this.m_Received = true;
		}


		private void EventReceived (object p1)
		{
			this.m_Received = true;
			if (!m_Argument1.isNone) {
				m_Argument1.Value = p1;
			} 
		}

		private void EventReceived (object p1, object p2)
		{
			this.m_Received = true;
			if (!m_Argument1.isNone) {
				m_Argument1.Value = p1;
			} 
			if (!m_Argument2.isNone) {
				m_Argument2.Value = p2;
			}
		}

		private void EventReceived (object p1, object p2, object p3)
		{
			this.m_Received = true;
			if (!m_Argument1.isNone) {
				m_Argument1.Value = p1;
			}  
			if (!m_Argument2.isNone) {
				m_Argument2.Value = p2;
			}  
			if (!m_Argument3.isNone) {
				m_Argument3.Value = p3;
			}
		}

		private int GetArgumentsCount ()
		{
			int count = 0;
			if (!m_Argument1.isNone) {
				count++;
			}
			if (!m_Argument2.isNone) {
				count++;
			}
			if (!m_Argument3.isNone) {
				count++;
			}
			return count;
		}
	}
}
