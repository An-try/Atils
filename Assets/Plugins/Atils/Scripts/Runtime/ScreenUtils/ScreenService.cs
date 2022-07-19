using UnityEngine;
using Zenject;

namespace Atils.Runtime.ScreenUtils
{
	public class ScreenService : MonoBehaviour, IScreenService
	{
		public ScreenSizeChangedHandler ScreenSizeChangedEvent { get; set; } = default;

		[SerializeField] private Vector2 _referenceSize = default;

		public Vector2 ReferenceSize => _referenceSize;
		public float ReferenceSizeRatio => _referenceSize.x / _referenceSize.y;

		public Vector2 CurrentSize { get; private set; } = default;
		public float CurrentSizeRatio => CurrentSize.x / CurrentSize.y;

		public float ScreenTotalSize => CurrentSize.x + CurrentSize.y;

		private Vector2 _previousScreenSize = default;

		[Inject]
		private void Construct()
		{
			CurrentSize = new Vector2(Screen.width, Screen.height);
			_previousScreenSize = CurrentSize;
		}

		private void Update()
		{
			CurrentSize = new Vector2(Screen.width, Screen.height);

			if (CurrentSize != _previousScreenSize)
			{
				_previousScreenSize = CurrentSize;
				ScreenSizeChangedEvent?.Invoke(CurrentSize);
			}
		}
	}
}
