using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class TransformExtensions
	{
		public static void SetX(this Transform source, float value)
		{
			source.position = source.position.GetSetX(value);
		}

		public static void SetY(this Transform source, float value)
		{
			source.position = source.position.GetSetY(value);
		}

		public static void SetZ(this Transform source, float value)
		{
			source.position = source.position.GetSetZ(value);
		}

		public static void SetXLocal(this Transform source, float value)
		{
			source.localPosition = source.localPosition.GetSetX(value);
		}

		public static void SetYLocal(this Transform source, float value)
		{
			source.localPosition = source.localPosition.GetSetY(value);
		}

		public static void SetZLocal(this Transform source, float value)
		{
			source.localPosition = source.localPosition.GetSetZ(value);
		}

		public static void DestroyChildrenSafely(this Transform source)
		{
			//save children to list and destroy them from this list
			while (source.childCount > 0)
			{
				source.GetChild(0).gameObject.DestroySafely();
			}
		}
	}
}
