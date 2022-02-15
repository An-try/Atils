using UnityEngine;
using UnityEngine.EventSystems;

namespace Atils.Runtime.Inputs
{
	public class MouseInputService : InputService
	{
		public bool IsMouseLeftHolding => UnityEngine.Input.GetKey(KeyCode.Mouse0);

		public override float PointerAxisX => UnityEngine.Input.GetAxis("Mouse X");
		public override float PointerAxisY => UnityEngine.Input.GetAxis("Mouse Y");
		public override float PointerPositionX => UnityEngine.Input.mousePosition.x;
		public override float PointerPositionY => UnityEngine.Input.mousePosition.y;
		public override bool IsPointerOverUIObject => EventSystem.current.IsPointerOverGameObject();
		public override bool IsAnyObjectSelectedAndHolding => EventSystem.current.currentSelectedGameObject != null && IsMouseLeftHolding;

		protected override void Update()
		{
			base.Update();

			float timeStep = Time.deltaTime;

			if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
			{
				OnPointerDownEvent?.Invoke(PointerPositionX, PointerPositionY);
				DetectSpatialButtonDown();
			}

			if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
			{
				OnPointerUpEvent?.Invoke(PointerPositionX, PointerPositionY);
				DetectSpatialButtonUp();
			}

			if (IsMouseLeftHolding)
			{
				OnPointerHoldEvent?.Invoke(PointerPositionX, PointerPositionY, timeStep);
				HandlePointerOverPressedSpatialButton();
			}

			float mouseScroll = UnityEngine.Input.mouseScrollDelta.y;
			if (mouseScroll != 0)
			{
				OnPointerScaleEvent?.Invoke(mouseScroll, timeStep);
			}
		}
	}
}
