using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.Reflection
{
	[Category ("Reflection")]
	[Tooltip ("Sets the field to the value specified. Returns success if the field was set.")]
	public class SetField : Action
	{
		[Tooltip ("The GameObject to set the field on")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The component to set the field on")]
		public StringVariable m_ComponentName;
		[Tooltip ("The name of the field")]
		public StringVariable m_FieldName;
		[Tooltip ("The value to set")]
		public GenericVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			var type = TypeUtility.GetType (m_ComponentName.Value);
			if (type == null) {
				Debug.LogWarning ("Unable to set field - type is null");
				return TaskStatus.Failure;
			}

			var component = m_gameObject.Value.GetComponent (type);
			if (component == null) {
				Debug.LogWarning ("Unable to set the field with component " + m_ComponentName.Value);
				return TaskStatus.Failure;
			}
			var field = component.GetType ().GetField (m_FieldName.Value);
			field.SetValue (component, m_value.sourceVariable.RawValue);

			return TaskStatus.Success;
		}
	}
}
