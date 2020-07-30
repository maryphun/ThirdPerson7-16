using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class GlobalVariables : ScriptableObject
	{
		[SerializeField]
		private FloatVariable[] m_FloatVariables;
		[SerializeField]
		private IntVariable[] m_IntVariables;
		[SerializeField]
		private BoolVariable[] m_BoolVariables;
		[SerializeField]
		private StringVariable[] m_StringVariables;
		[SerializeField]
		private GameObjectVariable[] m_GameObjectVariables;
		[SerializeField]
		private ColorVariable[] m_ColorVariables;
		[SerializeField]
		private MaterialVariable[] m_MaterialVariables;
		[SerializeField]
		private ObjectVariable[] m_ObjectVariables;
		[SerializeField]
		private TransformVariable[] m_TransformVariables;
		[SerializeField]
		private Vector2Variable[] m_Vector2Variables;
		[SerializeField]
		private Vector3Variable[] m_Vector3Variables;
		[SerializeField]
		private Vector4Variable[] m_Vector4Variables;
		[SerializeField]
		private GenericVariable[] m_GenericVariables;
		[SerializeField]
		private ArrayVariable[] m_ArrayVariables;
		[SerializeField]
		private SpriteVariable[] m_SpriteVariables;

		public FloatVariable[] floatVariables {
			get {
				if (this.m_FloatVariables == null) {
					this.m_FloatVariables = new FloatVariable[0]; 
				}
				return this.m_FloatVariables;
			}
			set {
				this.m_FloatVariables = value;
			}
		}

		public IntVariable[] intVariables {
			get {
				if (this.m_IntVariables == null) {
					this.m_IntVariables = new IntVariable[0]; 
				}
				return this.m_IntVariables;
			}
			set {
				this.m_IntVariables = value;
			}
		}

		public BoolVariable[] boolVariables {
			get {
				if (this.m_BoolVariables == null) {
					this.m_BoolVariables = new BoolVariable[0]; 
				}
				return this.m_BoolVariables;
			}
			set {
				this.m_BoolVariables = value;
			}
		}

		public StringVariable[] stringVariables {
			get {
				if (this.m_StringVariables == null) {
					this.m_StringVariables = new StringVariable[0]; 
				}
				return this.m_StringVariables;
			}
			set {
				this.m_StringVariables = value;
			}
		}

		public GameObjectVariable[] gameObjectVariables {
			get {
				if (this.m_GameObjectVariables == null) {
					this.m_GameObjectVariables = new GameObjectVariable[0]; 
				}
				return this.m_GameObjectVariables;
			}
			set {
				this.m_GameObjectVariables = value;
			}
		}

		public ColorVariable[] colorVariables {
			get {
				if (this.m_ColorVariables == null) {
					this.m_ColorVariables = new ColorVariable[0]; 
				}
				return this.m_ColorVariables;
			}
			set {
				this.m_ColorVariables = value;
			}
		}

		public MaterialVariable[] materialVariables {
			get {
				if (this.m_MaterialVariables == null) {
					this.m_MaterialVariables = new MaterialVariable[0]; 
				}
				return this.m_MaterialVariables;
			}
			set {
				this.m_MaterialVariables = value;
			}
		}

		public ObjectVariable[] objectVariables {
			get {
				if (this.m_ObjectVariables == null) {
					this.m_ObjectVariables = new ObjectVariable[0]; 
				}
				return this.m_ObjectVariables;
			}
			set {
				this.m_ObjectVariables = value;
			}
		}

		public TransformVariable[] transformVariables {
			get {
				if (this.m_TransformVariables == null) {
					this.m_TransformVariables = new TransformVariable[0]; 
				}
				return this.m_TransformVariables;
			}
			set {
				this.m_TransformVariables = value;
			}
		}

		public Vector2Variable[] vector2Variables {
			get {
				if (this.m_Vector2Variables == null) {
					this.m_Vector2Variables = new Vector2Variable[0]; 
				}
				return this.m_Vector2Variables;
			}
			set {
				this.m_Vector2Variables = value;
			}
		}

		public Vector3Variable[] vector3Variables {
			get {
				if (this.m_Vector3Variables == null) {
					this.m_Vector3Variables = new Vector3Variable[0]; 
				}
				return this.m_Vector3Variables;
			}
			set {
				this.m_Vector3Variables = value;
			}
		}

		public Vector4Variable[] vector4Variables {
			get {
				if (this.m_Vector4Variables == null) {
					this.m_Vector4Variables = new Vector4Variable[0]; 
				}
				return this.m_Vector4Variables;
			}
			set {
				this.m_Vector4Variables = value;
			}
		}

		public GenericVariable[] genericVariables {
			get {
				if (this.m_GenericVariables == null) {
					this.m_GenericVariables = new GenericVariable[0]; 
				}
				return this.m_GenericVariables;
			}
			set {
				this.m_GenericVariables = value;
			}
		}

		public ArrayVariable[] arrayVariables {
			get {
				if (this.m_ArrayVariables == null) {
					this.m_ArrayVariables = new ArrayVariable[0]; 
				}
				return this.m_ArrayVariables;
			}
			set {
				this.m_ArrayVariables = value;
			}
		}

		public SpriteVariable[] spriteVariables {
			get {
				if (this.m_SpriteVariables == null) {
					this.m_SpriteVariables = new SpriteVariable[0]; 
				}
				return this.m_SpriteVariables;
			}
			set {
				this.m_SpriteVariables = value;
			}
		}


		public Variable GetVariable (string name)
		{
			Variable[] variables = GetAllVariables ();
			for (int i = 0; i < variables.Length; i++) {
				Variable variable = variables [i];
				if (variable.name == name) {
					return variable;
				}
			}
			return null;
		}

		public void Add (Variable variable)
		{
			Type type = variable.GetType ();

			if (type == typeof(BoolVariable)) {
				Add<BoolVariable> (ref this.m_BoolVariables, variable);
			} else if (type == typeof(FloatVariable)) {
				Add<FloatVariable> (ref this.m_FloatVariables, variable);
			} else if (type == typeof(StringVariable)) {
				Add<StringVariable> (ref this.m_StringVariables, variable);
			} else if (type == typeof(GameObjectVariable)) {
				Add<GameObjectVariable> (ref this.m_GameObjectVariables, variable);
			} else if (type == typeof(IntVariable)) {
				Add<IntVariable> (ref this.m_IntVariables, variable);
			} else if (type == typeof(ColorVariable)) {
				Add<ColorVariable> (ref this.m_ColorVariables, variable);
			} else if (type == typeof(MaterialVariable)) {
				Add<MaterialVariable> (ref this.m_MaterialVariables, variable);
			} else if (type == typeof(ObjectVariable)) {
				Add<ObjectVariable> (ref this.m_ObjectVariables, variable);
			} else if (type == typeof(TransformVariable)) {
				Add<TransformVariable> (ref this.m_TransformVariables, variable);
			} else if (type == typeof(Vector2Variable)) {
				Add<Vector2Variable> (ref this.m_Vector2Variables, variable);
			} else if (type == typeof(Vector3Variable)) {
				Add<Vector3Variable> (ref this.m_Vector3Variables, variable);
			} else if (type == typeof(Vector4Variable)) {
				Add<Vector4Variable> (ref this.m_Vector4Variables, variable);
			} else if (type == typeof(GenericVariable)) {
				Add<GenericVariable> (ref this.m_GenericVariables, variable);
			} else if (type == typeof(ArrayVariable)) {
				Add<ArrayVariable> (ref this.m_ArrayVariables, variable);
			} else if (type == typeof(SpriteVariable)) {
				Add<SpriteVariable> (ref this.m_SpriteVariables, variable);
			}

		}

		public void AddRange (Variable[] variables)
		{
			for (int i = 0; i < variables.Length; i++) {
				Add (variables [i]);
			}
		}

		public void Remove (Variable variable)
		{
			Type type = variable.GetType ();

			if (type == typeof(BoolVariable)) {
				Remove<BoolVariable> (ref this.m_BoolVariables, variable);
			} else if (type == typeof(FloatVariable)) {
				Remove<FloatVariable> (ref this.m_FloatVariables, variable);
			} else if (type == typeof(StringVariable)) {
				Remove<StringVariable> (ref this.m_StringVariables, variable);
			} else if (type == typeof(GameObjectVariable)) {
				Remove<GameObjectVariable> (ref this.m_GameObjectVariables, variable);
			} else if (type == typeof(IntVariable)) {
				Remove<IntVariable> (ref this.m_IntVariables, variable);
			} else if (type == typeof(ColorVariable)) {
				Remove<ColorVariable> (ref this.m_ColorVariables, variable);
			} else if (type == typeof(MaterialVariable)) {
				Remove<MaterialVariable> (ref this.m_MaterialVariables, variable);
			} else if (type == typeof(ObjectVariable)) {
				Remove<ObjectVariable> (ref this.m_ObjectVariables, variable);
			} else if (type == typeof(TransformVariable)) {
				Remove<TransformVariable> (ref this.m_TransformVariables, variable);
			} else if (type == typeof(Vector2Variable)) {
				Remove<Vector2Variable> (ref this.m_Vector2Variables, variable);
			} else if (type == typeof(Vector3Variable)) {
				Remove<Vector3Variable> (ref this.m_Vector3Variables, variable);
			} else if (type == typeof(Vector4Variable)) {
				Remove<Vector4Variable> (ref this.m_Vector4Variables, variable);
			} else if (type == typeof(GenericVariable)) {
				Remove<GenericVariable> (ref this.m_GenericVariables, variable);
			} else if (type == typeof(ArrayVariable)) {
				Remove<ArrayVariable> (ref this.m_ArrayVariables, variable);
			} else if (type == typeof(SpriteVariable)) {
				Remove<SpriteVariable> (ref this.m_SpriteVariables, variable);
			}
		}

		public void Clear ()
		{
			Clear<BoolVariable> (ref this.m_BoolVariables);
			Clear<FloatVariable> (ref this.m_FloatVariables);
			Clear<StringVariable> (ref this.m_StringVariables);
			Clear<GameObjectVariable> (ref this.m_GameObjectVariables);
			Clear<IntVariable> (ref this.m_IntVariables);
			Clear<ColorVariable> (ref this.m_ColorVariables);
			Clear<MaterialVariable> (ref this.m_MaterialVariables);
			Clear<ObjectVariable> (ref this.m_ObjectVariables);
			Clear<TransformVariable> (ref this.m_TransformVariables);
			Clear<Vector2Variable> (ref this.m_Vector2Variables);
			Clear<Vector3Variable> (ref this.m_Vector3Variables);
			Clear<Vector4Variable> (ref this.m_Vector4Variables);
			Clear<GenericVariable> (ref this.m_GenericVariables);
			Clear<ArrayVariable> (ref this.m_ArrayVariables);
			Clear<SpriteVariable> (ref this.m_SpriteVariables);
		}

		private void Add<T> (ref T[] array, Variable item)
		{
			Array.Resize<T> (ref array, (int)array.Length + 1);
			array [(int)array.Length - 1] = (T)(object)item;
		}

		private void Remove<T> (ref T[] array, Variable item)
		{
			List<T> ts = new List<T> (array);
			ts.Remove ((T)(object)item);
			array = ts.ToArray ();
		}

		private void Clear<T> (ref T[] array)
		{
			Array.Clear (array, 0, (int)array.Length);
			Array.Resize<T> (ref array, 0);
		}

		public Variable[] GetVariablesOfType (Type type)
		{
			if (type == typeof(BoolVariable)) {
				return this.boolVariables;
			} else if (type == typeof(FloatVariable)) {
				return this.floatVariables;
			} else if (type == typeof(StringVariable)) {
				return this.stringVariables;
			} else if (type == typeof(GameObjectVariable)) {
				return this.gameObjectVariables;
			} else if (type == typeof(IntVariable)) {
				return this.intVariables;
			} else if (type == typeof(ColorVariable)) {
				return this.colorVariables;
			} else if (type == typeof(MaterialVariable)) {
				return this.materialVariables;
			} else if (type == typeof(ObjectVariable)) {
				return this.objectVariables;
			} else if (type == typeof(TransformVariable)) {
				return this.transformVariables;
			} else if (type == typeof(Vector2Variable)) {
				return this.vector2Variables;
			} else if (type == typeof(Vector3Variable)) {
				return this.vector3Variables;
			} else if (type == typeof(Vector4Variable)) {
				return this.vector4Variables;
			} else if (type == typeof(GenericVariable)) {
				return this.genericVariables;
			} else if (type == typeof(ArrayVariable)) {
				return this.arrayVariables;
			} else if (type == typeof(SpriteVariable)) {
				return this.spriteVariables;
			}
			return new Variable[0];
		}

		public Variable[] GetAllVariables ()
		{
			List<Variable> variables = new List<Variable> ();
			variables.AddRange (this.floatVariables);
			variables.AddRange (this.boolVariables);
			variables.AddRange (this.stringVariables);
			variables.AddRange (this.gameObjectVariables);
			variables.AddRange (this.intVariables);
			variables.AddRange (this.colorVariables);
			variables.AddRange (this.materialVariables);
			variables.AddRange (this.objectVariables);
			variables.AddRange (this.transformVariables);
			variables.AddRange (this.vector2Variables);
			variables.AddRange (this.vector3Variables);
			variables.AddRange (this.vector4Variables);
			variables.AddRange (this.genericVariables);
			variables.AddRange (this.arrayVariables);
			variables.AddRange (this.spriteVariables);
			return variables.OrderBy (x => x.index).ToArray ();
		}
	}
}