using UnityEditor;
using UnityEngine;

namespace Atils.Runtime.Attributes.Editor
{
	[CustomPropertyDrawer(typeof(OptionalAttribute))]
	public class OptionalDrawer : DecoratorDrawer
	{
		private static readonly float HEADER_SIZE_AS_PERCENT = 0.25f;

		public override float GetHeight()
		{
			return base.GetHeight() * (1f + HEADER_SIZE_AS_PERCENT);
		}

		public override void OnGUI(Rect position)
		{
			position.y += GetHeight() * HEADER_SIZE_AS_PERCENT / (1f + HEADER_SIZE_AS_PERCENT);
			EditorGUI.LabelField(position, "[Optional]");
		}
	}
}
