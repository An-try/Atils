using UnityEngine;
using UnityEngine.UIElements;

public static class VisualElementExtensions
{
	public static Vector3 TransformPoint(this VisualElement source, Vector2 position)
	{
		return source.WorldToLocal(position);
	}
}
