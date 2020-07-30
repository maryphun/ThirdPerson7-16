using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace DevionGames.BehaviorTrees
{
	public abstract class ObjectDecoratorDrawer
	{
		public object declaringObject;
		public FieldInfo fieldInfo;
		public object value;

		public virtual void OnGUI (GUIContent label)
		{

		}
	}
}