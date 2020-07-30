using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace DevionGames.BehaviorTrees
{
	public class VariableReference
	{
		public FieldInfo fieldInfo;
		public object declaringObject;
		public Variable variable;

		public VariableReference (FieldInfo fieldInfo, object declaringObject, Variable variable)
		{
			this.fieldInfo = fieldInfo;
			this.declaringObject = declaringObject;
			this.variable = variable;
		}

		public void Update (BehaviorTree behaviorTree)
		{
			if (variable is GenericVariable) {
				GenericVariable generic = variable as GenericVariable;
				if (generic.sourceVariable.isShared) {
					Variable reference = behaviorTree.blackboard.GetVariable (variable.name, true);
					generic.sourceVariable = reference;
				}
			} else if (variable.isShared && !variable.isNone) {
				Variable reference = behaviorTree.blackboard.GetVariable (variable.name, true);
				if (typeof(IList).IsAssignableFrom (fieldInfo.FieldType)) {
					IList list = (IList)fieldInfo.GetValue (declaringObject);
					for (int i = 0; i < list.Count; i++) {
						Variable v = list [i] as Variable;
						if (v.name == reference.name) {
							list [i] = reference;
							break;
						}
					}
					fieldInfo.SetValue (declaringObject, list);
					return;
				}
		
				fieldInfo.SetValue (declaringObject, reference);

			}
		}
	}
}