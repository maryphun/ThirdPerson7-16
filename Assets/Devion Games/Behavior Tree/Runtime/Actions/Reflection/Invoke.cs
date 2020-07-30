using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.Reflection
{
    [Category("Reflection")]
    [Tooltip("Invokes the specified method with the specified parameters. Returns success.")]
    public class Invoke : Action
    {
        [Tooltip("The GameObject to set the field on")]
        public GameObjectVariable m_gameObject;
        [Tooltip("The component to set the field on")]
        public StringVariable m_ComponentName;
        [Tooltip("The name of the field")]
        public StringVariable m_MethodName;
        [Tooltip("The first parameter of the method")]
        public GenericVariable m_Parameter1;
        [Tooltip("The second parameter of the method")]
        public GenericVariable m_Parameter2;
        [Tooltip("The third parameter of the method")]
        public GenericVariable m_Parameter3;
        [Tooltip("The fourth parameter of the method")]
        public GenericVariable m_Parameter4;

        public override TaskStatus OnUpdate()
        {
            var type = TypeUtility.GetType(m_ComponentName.Value);
            if (type == null)
            {
                Debug.LogWarning("Unable to invoke - type is null");
                return TaskStatus.Failure;
            }

            var component = m_gameObject.Value.GetComponent(type);
            if (component == null)
            {
                Debug.LogWarning("Unable to invoke with component " + m_ComponentName.Value);
                return TaskStatus.Failure;
            }

            List<object> parameterList = new List<object>();
            List<Type> typeList = new List<Type>();
            if (!m_Parameter1.isNone) {
                parameterList.Add(m_Parameter1.Value);
                typeList.Add(m_Parameter1.type);
            }
            if (!m_Parameter2.isNone)
            {
                parameterList.Add(m_Parameter2.Value);
                typeList.Add(m_Parameter2.type);
            }
            if (!m_Parameter3.isNone)
            {
                parameterList.Add(m_Parameter3.Value);
                typeList.Add(m_Parameter3.type);
            }
            if (!m_Parameter4.isNone)
            {
                parameterList.Add(m_Parameter4.Value);
                typeList.Add(m_Parameter4.type);
            }
            var methodInfo = component.GetType().GetMethod(this.m_MethodName.Value, typeList.ToArray());

            if (methodInfo == null) {
                Debug.LogWarning("Unable to invoke method " + this.m_MethodName.Value + " on component " + this.m_ComponentName.Value);
                return TaskStatus.Failure;
            }
            methodInfo.Invoke(component, parameterList.ToArray());

            return TaskStatus.Success;
        }
    }
}
