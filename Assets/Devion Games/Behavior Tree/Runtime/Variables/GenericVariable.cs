using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class GenericVariable : Variable
	{
		[SerializeField]
		private BoolVariable m_BoolValue;
		[SerializeField]
		private ColorVariable m_ColorValue;
		[SerializeField]
		private FloatVariable m_FloatValue;
		[SerializeField]
		private GameObjectVariable m_GameObjectValue;
		[SerializeField]
		private IntVariable m_IntValue;
		[SerializeField]
		private MaterialVariable m_MaterialValue;
		[SerializeField]
		private ObjectVariable m_ObjectValue;
		[SerializeField]
		public StringVariable m_StringValue;
		[SerializeField]
		private TransformVariable m_TransformValue;
		[SerializeField]
		private Vector2Variable m_Vector2Value;
		[SerializeField]
		private Vector3Variable m_Vector3Value;
		[SerializeField]
		private Vector4Variable m_Vector4Value;
		[SerializeField]
		private SpriteVariable m_SpriteValue;

		public object Value {
			get{ return this.RawValue; }
			set{ this.RawValue = value; }
		}

		public override object RawValue {
			get {
				return sourceVariable != null ? sourceVariable.RawValue : null;

			}
			set {
				if (sourceVariable != null) {
					sourceVariable.RawValue = value;
				}
			}
		}

		public Variable sourceVariable {
			get { 
				switch (this.m_VariableType) {
				case VariableType.Bool:
					return this.m_BoolValue;
				case VariableType.Color:
					return this.m_ColorValue;
				case VariableType.Float:
					return this.m_FloatValue;
				case VariableType.GameObject:
					return this.m_GameObjectValue;
				case VariableType.Int:
					return this.m_IntValue;
				case VariableType.Material:
					return this.m_MaterialValue;
				case VariableType.Object:
					return this.m_ObjectValue;
				case VariableType.String:
					return this.m_StringValue;
				case VariableType.Transform:
					return this.m_TransformValue;
				case VariableType.Vector2:
					return this.m_Vector2Value;
				case VariableType.Vector3:
					return this.m_Vector3Value;
				case VariableType.Vector4:
					return this.m_Vector4Value;
				case VariableType.Sprite:
					return this.m_SpriteValue;
				default:
					return null;
				}
			}
			set { 
				System.Type type = value.GetType ();

				if (type == typeof(BoolVariable)) {
					this.m_BoolValue = (BoolVariable)value;
				} else if (type == typeof(FloatVariable)) {
					this.m_FloatValue = (FloatVariable)value;
				} else if (type == typeof(StringVariable)) {
					this.m_StringValue = (StringVariable)value;
				} else if (type == typeof(GameObjectVariable)) {
					this.m_GameObjectValue = (GameObjectVariable)value;
				} else if (type == typeof(IntVariable)) {
					this.m_IntValue = (IntVariable)value;
				} else if (type == typeof(ColorVariable)) {
					this.m_ColorValue = (ColorVariable)value;
				} else if (type == typeof(MaterialVariable)) {
					this.m_MaterialValue = (MaterialVariable)value;
				} else if (type == typeof(ObjectVariable)) {
					this.m_ObjectValue = (ObjectVariable)value;
				} else if (type == typeof(TransformVariable)) {
					this.m_TransformValue = (TransformVariable)value;
				} else if (type == typeof(Vector2Variable)) {
					this.m_Vector2Value = (Vector2Variable)value;
				} else if (type == typeof(Vector3Variable)) {
					this.m_Vector3Value = (Vector3Variable)value;
				} else if (type == typeof(Vector4Variable)) {
					this.m_Vector4Value = (Vector4Variable)value;
				} else if (type == typeof(SpriteVariable)) {
					this.m_SpriteValue = (SpriteVariable)value;
				} 
			}
		}

		[SerializeField]
		private VariableType m_VariableType;

		public VariableType variableType {
			get { 
				return this.m_VariableType;
			}
			set { 
				this.m_VariableType = value;
			}
		}

		public override System.Type type {
			get {
				return  sourceVariable != null ? sourceVariable.type : null;
			}
		}

		public override bool isShared {
			get {
				return  sourceVariable != null ? sourceVariable.isShared : false;
			}
			set {
				if (sourceVariable != null) {
					sourceVariable.isShared = value;
				} else {
					base.isShared = value;
				}
			}
		}

		public override bool isNone {
			get {
				return sourceVariable != null ? sourceVariable.isNone : true;
			}
		}

		public override string name {
			get {
				return sourceVariable != null ? sourceVariable.name : base.name;
			}
			set {
				if (sourceVariable != null) {
					sourceVariable.name = value;
				} else {
					base.name = value;
				}
			}
		}

		public GenericVariable ()
		{
		}

		public GenericVariable (string name) : base (name)
		{
		}

		public GenericVariable (GenericVariable source) : base (source)
		{
			if (source != null) {
				this.m_VariableType = source.m_VariableType;
				this.RawValue = source.RawValue;
			}
		}

		public override Variable Clone ()
		{
			return new GenericVariable (this);
		}

		public System.Type GetVariableSourceType ()
		{
			switch (this.m_VariableType) {
			case VariableType.Bool:
				return typeof(BoolVariable);
			case VariableType.Color:
				return typeof(ColorVariable);
			case VariableType.Float:
				return typeof(FloatVariable);
			case VariableType.GameObject:
				return typeof(GameObjectVariable);
			case VariableType.Int:
				return typeof(IntVariable);
			case VariableType.Material:
				return typeof(MaterialVariable);
			case VariableType.Object:
				return typeof(ObjectVariable);
			case VariableType.String:
				return typeof(StringVariable);
			case VariableType.Transform:
				return typeof(TransformVariable);
			case VariableType.Vector2:
				return typeof(Vector2Variable);
			case VariableType.Vector3:
				return typeof(Vector3Variable);
			case VariableType.Vector4:
				return typeof(Vector4Variable);
			case VariableType.Sprite:
				return typeof(SpriteVariable);
			default:
				return null;
			}
		}
	}
}