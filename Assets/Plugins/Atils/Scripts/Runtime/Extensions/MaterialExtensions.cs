using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class MaterialExtensions
	{
		public static Material Copy(this Material source)
		{
			Material copy = new Material(source);
			copy.name += " (Instance)";
			return copy;
		}
	}
}
