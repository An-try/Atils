using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Atils.Runtime.Inputs
{
	public class OVRInputService : InputService
	{
		[Inject] private IOVRRaycaster _ovrRaycaster = default;

		public override string Name => "OVR input service";

		public bool IsMouseLeftHolding => throw new NotImplementedException();

		public override float PointerAxisX => throw new NotImplementedException();
		public override float PointerAxisY => throw new NotImplementedException();
		public override float PointerPositionX => throw new NotImplementedException();
		public override float PointerPositionY => throw new NotImplementedException();
		public override bool IsPointerOverUIObject => IsPointerOverUIObjectRaycast();
		public override bool IsAnyObjectSelectedAndHolding => EventSystem.current.currentSelectedGameObject != null && IsMouseLeftHolding;

		protected override SpatialButton TryGetSpatialButtonUnderCursor()
		{
			RaycastHit raycastHit = _ovrRaycaster.RaycastHit;

			if (raycastHit.transform != null)
			{
				return raycastHit.transform.GetComponent<SpatialButton>();
			}

			return null;
		}

		protected override bool IsPointerOverUIObjectRaycast()
		{
			return false;
		}

		protected override void HandlePointerOverPressedSpatialButton()
		{
			IsPointerOverPressedSpatialButton = false;
		}
	}
}
