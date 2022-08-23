using UnityEngine;

namespace Atils.Runtime.ScreenUtils
{
	public interface IScreenService
	{
		ScreenSizeChangedHandler ScreenSizeChangedEvent { get; set; }

		Vector2 ReferenceSize { get; }
		float ReferenceSizeRatio { get; }

		Vector2 CurrentSize { get; }
		float CurrentSizeRatio { get; }

		float ScreenTotalSize { get; }
	}
}
