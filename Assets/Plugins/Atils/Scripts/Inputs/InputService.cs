using Atils.Runtime.ScreenUtils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Atils.Runtime.Inputs
{
	public abstract class InputService : MonoBehaviour, IInputService
	{
		public abstract string Name { get; }

		public PointerDownHandler OnPointerDownEvent { get; set; } = default;
		public PointerUpHandler OnPointerUpEvent { get; set; } = default;
		public PointerHoldHandler OnPointerHoldEvent { get; set; } = default;
		public PointerScaleHandler OnPointerScaleEvent { get; set; } = default;
		public SpatialButtonEnterHandler OnSpatialButtonEnterEvent { get; set; } = default;
		public SpatialButtonExitHandler OnSpatialButtonExitEvent { get; set; } = default;
		public SpatialButtonDownHandler OnSpatialButtonDownEvent { get; set; } = default;
		public SpatialButtonUpHandler OnSpatialButtonUpEvent { get; set; } = default;
		public SpatialButtonClickHandler OnSpatialButtonClickEvent { get; set; } = default;

		public SpatialButton SpatialButtonDown { get; protected set; } = null;
		public SpatialButton SpatialButtonUnderCursor { get; protected set; } = null;
		public SpatialButton PreviousSpatialButtonUnderCursor { get; protected set; } = null;
		public bool IsPointerOverPressedSpatialButton { get; protected set; } = false;
		public bool IsPointerOverSpatialButton => SpatialButtonUnderCursor != null;

		public abstract float PointerAxisX { get; }
		public abstract float PointerAxisY { get; }
		public abstract float PointerPositionX { get; }
		public abstract float PointerPositionY { get; }
		public abstract bool IsPointerOverUIObject { get; }
		public abstract bool IsAnyObjectSelectedAndHolding { get; }
		public virtual Vector2 PointerPosition => new Vector2(PointerPositionX, PointerPositionY);

		[Inject] private IScreenService _screenService = default;

		private float BUTTON_CLICK_IF_POINTER_NOT_MOVING_DISTANCE_MULTIPLIER = 20;

		private Vector2 _pointerPositionOnButtonDown = default;
		private List<RaycastResult> _pointerOverUIRaycastResults = new List<RaycastResult>();

		protected virtual void Start()
		{
			if (EventSystem.current == null)
			{
				Debug.LogError(typeof(InputService) + ": Event System does not exist.");
			}
		}

		protected virtual void Update()
		{
			DetectSpatialButtonUnderCursor();
			DetectSpatialButtonEnterOrExit();
		}

		protected virtual void DetectSpatialButtonDown()
		{
			if (SpatialButtonUnderCursor != null && !IsPointerOverUIObject)
			{
				SpatialButtonDown = SpatialButtonUnderCursor;
				SpatialButtonDown.OnDown();
				OnSpatialButtonDownEvent?.Invoke(SpatialButtonDown);
				_pointerPositionOnButtonDown = PointerPosition;
				IsPointerOverPressedSpatialButton = true;
			}
		}

		protected virtual void DetectSpatialButtonUp()
		{
			if (SpatialButtonUnderCursor != null && !IsPointerOverUIObject)
			{
				SpatialButtonUnderCursor.OnUp();
				OnSpatialButtonUpEvent?.Invoke(SpatialButtonUnderCursor);
				DetectSpatialButtonClick();
				SpatialButtonDown = null;
			}
		}

		protected virtual void DetectSpatialButtonClick()
		{
			if (SpatialButtonUnderCursor == SpatialButtonDown && IsPointerOverPressedSpatialButton)
			{
				SpatialButtonUnderCursor.OnClick();
				OnSpatialButtonClickEvent?.Invoke(SpatialButtonUnderCursor);
			}
		}

		protected virtual void DetectSpatialButtonUnderCursor()
		{
			if (SpatialButtonUnderCursor != null && IsPointerOverUIObject)
			{
				SpatialButtonUnderCursor = null;
				return;
			}

			SpatialButtonUnderCursor = TryGetSpatialButtonUnderCursor();
		}

		protected virtual SpatialButton TryGetSpatialButtonUnderCursor()
		{
			Camera camera = UnityEngine.Camera.main;
			Ray ray = camera != null ? UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition) : default;

			if (Physics.Raycast(ray, out RaycastHit hit) && !IsPointerOverUIObject)
			{
				return hit.transform.GetComponent<SpatialButton>();
			}

			return null;
		}

		protected virtual void DetectSpatialButtonEnterOrExit()
		{
			if (SpatialButtonUnderCursor != PreviousSpatialButtonUnderCursor)
			{
				if (PreviousSpatialButtonUnderCursor != null)
				{
					PreviousSpatialButtonUnderCursor.OnExit();
					OnSpatialButtonExitEvent?.Invoke(PreviousSpatialButtonUnderCursor);
					IsPointerOverPressedSpatialButton = false;
				}

				if (SpatialButtonUnderCursor != null)
				{
					SpatialButtonUnderCursor.OnEnter();
					OnSpatialButtonEnterEvent?.Invoke(SpatialButtonUnderCursor);
				}

				PreviousSpatialButtonUnderCursor = SpatialButtonUnderCursor;
			}
		}

		protected virtual bool IsPointerOverUIObjectRaycast()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = new Vector2(PointerPositionX, PointerPositionY);

			EventSystem.current.RaycastAll(pointerEventData, _pointerOverUIRaycastResults);
			int raycastResultsCount = _pointerOverUIRaycastResults.Count;
			_pointerOverUIRaycastResults.Clear();

			return raycastResultsCount > 0;
		}

		protected virtual void HandlePointerOverPressedSpatialButton()
		{
			if (IsPointerOverPressedSpatialButton &&
				Vector2.Distance(PointerPosition, _pointerPositionOnButtonDown) >
					_screenService.CurrentSizeRatio * BUTTON_CLICK_IF_POINTER_NOT_MOVING_DISTANCE_MULTIPLIER)
			{
				IsPointerOverPressedSpatialButton = false;
			}
		}
	}
}
