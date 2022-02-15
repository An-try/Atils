using UnityEngine;

namespace Atils.Runtime.ScreenUtils
{
	public interface IScreenService
	{
		ScreenSizeChangedHandler ScreenSizeChangedEvent { get; set; }

		Vector2 CurrentScreenSize { get; }

		float ScreenTotalSize { get; }
	}
}
