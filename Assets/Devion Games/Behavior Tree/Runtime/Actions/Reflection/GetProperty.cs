using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.Reflection
{
    [Category("Reflection")]
    [Tooltip("Gets the value from the property specified. Returns success if the property was retrieved.")]
    public class GetProperty : Action
    {
        [Tooltip("The GameObject to get the property of")]
        public GameObjectVariable m_gameObject;
        [Tooltip("The component to get the property of")]
        public StringVariable componentName;
        [Tooltip("The name of the property")]
        public StringVariable propertyName;
        [Tooltip("The value of the property")]
        [Shared]
        public GenericVariable propertyValue;

        public override TaskStatus OnUpdate()
        {
            Type type = TypeUtility.GetType(componentName.Value);
            if (type == null)
            {
                Debug.LogWarning("Unable to get property - type is null");
                return TaskStatus.Failure;
            }

            var component = this.m_gameObject.Value.GetComponent(type);
            if (component == null)
            {
                Debug.LogWarning("Unable to get the property with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            var property = component.GetType().GetProperty(propertyName.Value);
            propertyValue.Value=property.GetValue(component, null);

            return TaskStatus.Success;
        }
    }
}