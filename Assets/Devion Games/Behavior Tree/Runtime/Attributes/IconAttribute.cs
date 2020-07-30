using System;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[AttributeUsage (AttributeTargets.Class)]
	public sealed class IconAttribute : Attribute
	{
		private readonly string path;
        private readonly Type type;

		public string Path {
			get {
				return this.path;
			}
		}

        public Type Type {
            get {
                return this.type;
            }
        }

		public IconAttribute (string path)
		{
			this.path = path;
		}

        public IconAttribute(Type type)
        {
            this.type = type;
        }
    }
}