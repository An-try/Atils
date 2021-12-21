using UnityEngine;
using UnityEngine.EventSystems;

namespace Atils.Runtime.Inputs
{
	public class MouseInputService : InputService, IInputService
	{
		public PointerHoldHandler OnPointerHoldEvent { get; set; } = default;
		public PointerScaleHandler OnPointerScaleEvent { get; set; } = default;
		public SpatialButtonEnterHandler OnSpatialButtonEnterEvent { get; set; } = default;
		public SpatialButtonExitHandler OnSpatialButtonExitEvent { get; set; } = default;
		public SpatialButtonClickHandler OnSpatialButtonClickEvent { get; set; } = default;

		public float PointerPositionX => UnityEngine.Input.GetAxis("Mouse X");
		public float PointerPositionY => UnityEngine.Input.GetAxis("Mouse Y");
		public bool IsPointerOverGameObject => EventSystem.current.IsPointerOverGameObject();
		public SpatialButton SpatialButtonUnderCursor { get; private set; } = default;
		public SpatialButton PreviousSpatialButtonUnderCursor { get; private set; } = default;

		private void Update()
		{
			float timeStep = Time.deltaTime;

			SpatialButtonUnderCursor = DetectSpatialButtonUnderCursor();
			PreviousSpatialButtonUnderCursor = DetectSpatialButtonEnterOrExit(SpatialButtonUnderCursor, PreviousSpatialButtonUnderCursor, OnSpatialButtonEnterEvent, OnSpatialButtonExitEvent);

			#region Spatial button click
			if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
			{
				if (SpatialButtonUnderCursor != null)
				{
					SpatialButtonUnderCursor.OnClick();
					OnSpatialButtonClickEvent?.Invoke(SpatialButtonUnderCursor);
				}
			}
			#endregion

			if (UnityEngine.Input.GetKey(KeyCode.Mouse0))
			{
				OnPointerHoldEvent?.Invoke(PointerPositionX, PointerPositionY, timeStep);
			}

			float mouseScroll = UnityEngine.Input.mouseScrollDelta.y;
			if (mouseScroll != 0)
			{
				OnPointerScaleEvent?.Invoke(mouseScroll, timeStep);
			}
		}
	}
}
