using System;

namespace DevionGames.BehaviorTrees
{
	[AttributeUsage (AttributeTargets.Class)]
	public sealed class HelpURLAttribute : Attribute
	{
		private readonly string url;

		public string URL {
			get {
				return this.url;
			}
		}

		public HelpURLAttribute (string url)
		{
			this.url = url;
		}
	}
}