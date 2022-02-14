using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Atils.Runtime.Inputs
{
	public abstract class InputService : MonoBehaviour, IInputService
	{
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
		public bool IsPointerOverSpatialButton => SpatialButtonUnderCursor != null;

		public abstract float PointerPositionX { get; }
		public abstract float PointerPositionY { get; }
		public abstract bool IsPointerOverUIObject { get; }
		public abstract bool IsAnyObjectSelectedAndHolding { get; }

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
			if (SpatialButtonUnderCursor == SpatialButtonDown)
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
			Ray ray = UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

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
	}
}
