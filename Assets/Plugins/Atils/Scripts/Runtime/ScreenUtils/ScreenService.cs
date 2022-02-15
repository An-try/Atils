using UnityEngine;
using Zenject;

namespace Atils.Runtime.ScreenUtils
{
	public class ScreenService : MonoBehaviour, IScreenService
	{
		public ScreenSizeChangedHandler ScreenSizeChangedEvent { get; set; } = default;

		public Vector2 CurrentScreenSize { get; private set; } = default;

		public float ScreenTotalSize => CurrentScreenSize.x + CurrentScreenSize.y;

		private Vector2 _previousScreenSize = default;

		[Inject]
		private void Construct()
		{
			CurrentScreenSize = new Vector2(Screen.width, Screen.height);
			_previousScreenSize = CurrentScreenSize;
		}

		private void Update()
		{
			CurrentScreenSize = new Vector2(Screen.width, Screen.height);

			if (CurrentScreenSize != _previousScreenSize)
			{
				_previousScreenSize = CurrentScreenSize;
				ScreenSizeChangedEvent?.Invoke(CurrentScreenSize);
			}
		}
	}
}
