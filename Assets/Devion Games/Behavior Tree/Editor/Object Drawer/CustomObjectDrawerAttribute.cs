using System;

namespace DevionGames.BehaviorTrees
{
	[AttributeUsage (AttributeTargets.All, AllowMultiple = false)]
	public sealed class CustomObjectDrawerAttribute : Attribute
	{
		private readonly Type type;

		public Type Type {
			get {
				return this.type;
			}
		}

		public CustomObjectDrawerAttribute (Type type)
		{
			this.type = type;
		}
	}
}