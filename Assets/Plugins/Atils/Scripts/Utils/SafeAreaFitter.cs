using UnityEngine;

namespace Atils.Runtime.Utils
{
	public class SafeAreaFitter : MonoBehaviour
	{
		private void Awake()
		{
			RectTransform rectTransform = GetComponent<RectTransform>();
			Rect safeArea = Screen.safeArea;
			safeArea.width -= 40;
			Vector2 anchorMin = safeArea.position;
			Vector2 anchorMax = anchorMin + safeArea.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			rectTransform.anchorMin = anchorMin;
			rectTransform.anchorMax = anchorMax;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
	}
}
