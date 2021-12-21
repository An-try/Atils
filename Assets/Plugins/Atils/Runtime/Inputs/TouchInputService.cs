using UnityEngine;
using UnityEngine.EventSystems;

namespace Atils.Runtime.Inputs
{
	public class TouchInputService : InputService, IInputService
    {
		private float _lastPositionX = default;
		private float _lastPositionY = default;

		private float _touchDistance = default;
		private float _lastTouchDistance = default;

		private bool _firstSingleTouch = false;
		private bool _firstBothTouch = false;

		public PointerHoldHandler OnPointerHoldEvent { get; set; } = default;
        public PointerScaleHandler OnPointerScaleEvent { get; set; } = default;
		public SpatialButtonEnterHandler OnSpatialButtonEnterEvent { get; set; } = default;
		public SpatialButtonExitHandler OnSpatialButtonExitEvent { get; set; } = default;
		public SpatialButtonClickHandler OnSpatialButtonClickEvent { get; set; } = default;

		public float PointerPositionX => TouchCount == 1 ? GetTouch(0).position.x : 0;
        public float PointerPositionY => TouchCount == 1 ? GetTouch(0).position.y : 0;
        public bool IsPointerOverGameObject => TouchCount >= 1 ? EventSystem.current.IsPointerOverGameObject(0) : false;
		public SpatialButton SpatialButtonUnderCursor { get; private set; } = default;
		public SpatialButton PreviousSpatialButtonUnderCursor { get; private set; } = default;

		public int TouchCount => UnityEngine.Input.touchCount;

		private void Update()
		{
			float timeStep = Time.deltaTime;

			SpatialButtonUnderCursor = DetectSpatialButtonUnderCursor();
			PreviousSpatialButtonUnderCursor = DetectSpatialButtonEnterOrExit(SpatialButtonUnderCursor, PreviousSpatialButtonUnderCursor, OnSpatialButtonEnterEvent, OnSpatialButtonExitEvent);

			#region Spatial button click
			if (TouchCount == 1 && GetTouch(0).phase == TouchPhase.Began)
			{
				if (SpatialButtonUnderCursor != null)
				{
					SpatialButtonUnderCursor.OnClick();
					OnSpatialButtonClickEvent?.Invoke(SpatialButtonUnderCursor);
				}
			}
			#endregion

			if (TouchCount == 1)
			{
				if (!_firstSingleTouch)
				{
					_lastPositionX = PointerPositionX;
					_lastPositionY = PointerPositionY;
				}

				float differenceX = (PointerPositionX - _lastPositionX) / Screen.width;
				float differenceY = (PointerPositionY - _lastPositionY) / Screen.height;

				OnPointerHoldEvent?.Invoke(differenceX, differenceY, timeStep, 50);

				_lastPositionX = PointerPositionX;
				_lastPositionY = PointerPositionY;
				_firstSingleTouch = true;
			}
			else
			{
				_firstSingleTouch = false;
			}

			if (TouchCount == 2)
			{
				Touch firstTouch = GetTouch(0);
				Touch secondTouch = GetTouch(1);

				if (!_firstBothTouch)
				{
					_lastTouchDistance = Vector2.Distance(firstTouch.position, secondTouch.position);
				}

				float newDist = Vector2.Distance(firstTouch.position, secondTouch.position);
				_touchDistance = _lastTouchDistance - newDist;
				_lastTouchDistance = newDist;

				OnPointerScaleEvent?.Invoke(-_touchDistance, timeStep, 0.01f);
				_firstBothTouch = true;
			}
			else
			{
				_firstBothTouch = false;
			}
		}

		private Touch GetTouch(int touchIndex)
		{
			return UnityEngine.Input.GetTouch(touchIndex);
		}
    }
}
