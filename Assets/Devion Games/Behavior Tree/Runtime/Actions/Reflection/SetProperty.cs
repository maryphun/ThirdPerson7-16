using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.Reflection
{
	[Category ("Reflection")]
	[Tooltip ("Sets the property to the value specified. Returns success if the property was set.")]
	public class SetProperty : Action
	{
		[Tooltip ("The GameObject to set the field on")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The component to set the field on")]
		public StringVariable m_ComponentName;
		[Tooltip ("The name of the field")]
		public StringVariable m_PropertyName;
		[Tooltip ("The value to set")]
		public GenericVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			var type = TypeUtility.GetType (m_ComponentName.Value);
			if (type == null) {
				Debug.LogWarning ("Unable to set property - type is null");
				return TaskStatus.Failure;
			}

			var component = m_gameObject.Value.GetComponent (type);
			if (component == null) {
				Debug.LogWarning ("Unable to set the property with component " + m_ComponentName.Value);
				return TaskStatus.Failure;
			}
			var property = component.GetType ().GetProperty (m_PropertyName.Value);
			property.SetValue (component, m_value.sourceVariable.RawValue, null);

			return TaskStatus.Success;
		}
	}
}
