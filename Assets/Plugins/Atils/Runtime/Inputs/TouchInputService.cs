using Atils.Runtime.WebGL;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Atils.Runtime.Inputs
{
	public class TouchInputService : InputService
    {
		private float _lastPositionX = default;
		private float _lastPositionY = default;

		private float _touchDistance = default;
		private float _lastTouchDistance = default;

		private bool _firstSingleTouch = false;
		private bool _firstBothTouch = false;

		public int TouchCount => UnityEngine.Input.touchCount;

		public override float PointerPositionX => TouchCount == 1 ? GetTouch(0).position.x : 0;
		public override float PointerPositionY => TouchCount == 1 ? GetTouch(0).position.y : 0;
		public override bool IsPointerOverUIObject => TouchCount >= 1 ? IsPointerOverGameObject(0) : false;
		public override bool IsAnyObjectSelectedAndHolding => EventSystem.current.currentSelectedGameObject != null && TouchCount > 1;

		private bool _isIOS = default;

		protected override void Start()
		{
			base.Start();

			_isIOS = PlatformProvider.IsIOS();
		}

		protected override void Update()
		{
			base.Update();

			float timeStep = Time.deltaTime;

			if (TouchCount == 1 && GetTouch(0).phase == TouchPhase.Began)
			{
				OnPointerDownEvent?.Invoke(PointerPositionX, PointerPositionY);
				DetectSpatialButtonDown();
			}

			if (TouchCount == 1 && GetTouch(0).phase == TouchPhase.Ended)
			{
				OnPointerUpEvent?.Invoke(PointerPositionX, PointerPositionY);
				DetectSpatialButtonUp();
			}

			if (TouchCount == 1)
			{
				if (!_firstSingleTouch)
				{
					_lastPositionX = PointerPositionX;
					_lastPositionY = PointerPositionY;
				}

				float differenceX = (PointerPositionX - _lastPositionX) / Screen.width;
				float differenceY = (PointerPositionY - _lastPositionY) / Screen.height;

				OnPointerHoldEvent?.Invoke(differenceX, differenceY, timeStep, 150);

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

		protected override void DetectSpatialButtonUnderCursor()
		{
			if (TouchCount == 1)
			{
				base.DetectSpatialButtonUnderCursor();
				return;
			}

			SpatialButtonUnderCursor = null;
		}

		protected override void DetectSpatialButtonUp()
		{
			SpatialButton spatialButton = TryGetSpatialButtonUnderCursor();

			if (spatialButton != null && !IsPointerOverUIObject)
			{
				DetectSpatialButtonClick(spatialButton);
			}
		}

		private void DetectSpatialButtonClick(SpatialButton spatialButton)
		{
			if (spatialButton != null && spatialButton == SpatialButtonDown)
			{
				spatialButton.OnClick();
				OnSpatialButtonClickEvent?.Invoke(spatialButton);
			}
		}

		private bool IsPointerOverGameObject(int pointerId)
		{
			return _isIOS ? IsPointerOverUIObjectRaycast() : EventSystem.current.IsPointerOverGameObject(pointerId);
		}

		private Touch GetTouch(int touchIndex)
		{
			return UnityEngine.Input.GetTouch(touchIndex);
		}
	}
}
