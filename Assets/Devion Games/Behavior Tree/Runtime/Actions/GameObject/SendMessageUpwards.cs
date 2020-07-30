using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject.SendMessageUpwards.html")]
	public class SendMessageUpwards : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The name of the method to call.")]
		public StringVariable m_MethodName;
		[Tooltip ("An optional parameter value to pass to the called method.")]
		public GenericVariable parameter;
		[Tooltip ("Should an error be raised if the method doesn't exist on the target object?")]
		public SendMessageOptions options;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_gameObject.Value.SendMessageUpwards (m_MethodName.Value, parameter.Value, options);
			return TaskStatus.Success;
		}
	}
}