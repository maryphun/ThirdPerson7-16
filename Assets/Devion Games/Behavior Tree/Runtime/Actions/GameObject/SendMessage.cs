using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Calls the method named methodName on every MonoBehaviour in the game object.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.SendMessage.html")]
	public class SendMessage : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The name of the method to call.")]
		public StringVariable m_MethodName;
		[Tooltip ("An optional argument value to pass to the called method.")]
		public GenericVariable argument;
		[Tooltip ("Should an error be raised if the method doesn't exist on the target object?")]
		public SendMessageOptions options;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}

			m_gameObject.Value.SendMessage (m_MethodName.Value, argument.Value, options);

			return TaskStatus.Success;
		}
	}
}