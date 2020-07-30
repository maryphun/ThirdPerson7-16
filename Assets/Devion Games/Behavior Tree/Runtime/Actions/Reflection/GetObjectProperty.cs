using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.Reflection
{
    [Category("Reflection")]
    [Tooltip("Gets the value from the property specified. Returns success if the property was retrieved.")]
    public class GetObjectProperty : Action
    {
        [Tooltip("The Object to get the property of")]
        public ObjectVariable m_Object;
        [Tooltip("The name of the property")]
        public StringVariable propertyName;
        [Tooltip("The value of the property")]
        [Shared]
        public GenericVariable propertyValue;

        public override TaskStatus OnUpdate()
        {
            var property = this.m_Object.Value.GetType().GetProperty(propertyName.Value);
            propertyValue.Value=property.GetValue(m_Object.Value, null);
            return TaskStatus.Success;
        }
    }
}