using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions
{
	[Category ("Behavior")]
	[Tooltip ("Send an event to target game object.")]
	public class SendEvent : Action
	{
		[NotRequired]
		[Tooltip ("Send the event to game object or leave empty to send the event to all game objects.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Event name")]
		public StringVariable m_EventName;

		[NotRequired]
		[Tooltip ("Optional Argument")]
		public GenericVariable m_Argument1;
		[NotRequired]
		[Tooltip ("Optional Argument")]
		public GenericVariable m_Argument2;
		[NotRequired]
		[Tooltip ("Optional Argument")]
		public GenericVariable m_Argument3;


		public override TaskStatus OnUpdate ()
		{
			List<object> arguments = GetArguments ();

			if (m_gameObject.isNone || m_gameObject.Value == null) {
				if (arguments.Count == 0) {
					Events.Invoke (m_EventName.Value);
				} else if (arguments.Count == 1) {
					Events.Invoke<object> (m_EventName.Value, arguments [0]);
				} else if (arguments.Count == 2) {
					Events.Invoke<object,object> (m_EventName.Value, arguments [0], arguments [1]);
				} else if (arguments.Count == 3) {
					Events.Invoke<object,object,object> (m_EventName.Value, arguments [0], arguments [1], arguments [2]);
				}

			} else {
				if (arguments.Count == 0) {
					Events.Invoke (m_gameObject.Value, m_EventName.Value);
				} else if (arguments.Count == 1) {
					Events.Invoke<object> (m_gameObject.Value, m_EventName.Value, arguments [0]);
				} else if (arguments.Count == 2) {
					Events.Invoke<object,object> (m_gameObject.Value, m_EventName.Value, arguments [0], arguments [1]);
				} else if (arguments.Count == 3) {
					Events.Invoke<object,object,object> (m_gameObject.Value, m_EventName.Value, arguments [0], arguments [1], arguments [2]);
				}
			}
			return TaskStatus.Success;
		}

		private List<object> GetArguments ()
		{
			List<object> arguments = new List<object> ();
			if (!m_Argument1.isNone) {
				arguments.Add (m_Argument1.Value);
			}
			if (!m_Argument2.isNone) {
				arguments.Add (m_Argument2.Value);
			}
			if (!m_Argument3.isNone) {
				arguments.Add (m_Argument3.Value);
			}
			return arguments;
		}

	}
}
