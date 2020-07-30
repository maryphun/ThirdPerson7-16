using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.Reflection
{
    [Category("Reflection")]
    [Tooltip("Gets the value from the field specified. Returns success if the property was retrieved.")]
    public class GetField : Action
    {
        [Tooltip("The GameObject to get the property of")]
        public GameObjectVariable m_gameObject;
        [Tooltip("The component to get the property of")]
        public StringVariable componentName;
        [Tooltip("The name of the field")]
        public StringVariable fieldName;
        [Tooltip("The value of the property")]
        [Shared]
        public GenericVariable fieldValue;

        public override TaskStatus OnUpdate()
        {
            Type type = TypeUtility.GetType(componentName.Value);
            if (type == null)
            {
                Debug.LogWarning("Unable to get field - type is null");
                return TaskStatus.Failure;
            }

            var component = this.m_gameObject.Value.GetComponent(type);
            if (component == null)
            {
                Debug.LogWarning("Unable to get the field with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            var field = component.GetType().GetField(fieldName.Value);
            fieldValue.Value = field.GetValue(component);

            return TaskStatus.Success;
        }
    }
}