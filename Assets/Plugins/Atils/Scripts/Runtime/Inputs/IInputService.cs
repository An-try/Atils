using System;
using UnityEngine;

namespace Atils.Runtime.Inputs
{
	public interface IInputService
	{
		string Name { get; }

		PointerDownHandler OnPointerDownEvent { get; set; }
		PointerUpHandler OnPointerUpEvent { get; set; }
		PointerHoldHandler OnPointerHoldEvent { get; set; }
		PointerScaleHandler OnPointerScaleEvent { get; set; }
		SpatialButtonEnterHandler OnSpatialButtonEnterEvent { get; set; }
		SpatialButtonExitHandler OnSpatialButtonExitEvent { get; set; }
		SpatialButtonDownHandler OnSpatialButtonDownEvent { get; set; }
		SpatialButtonUpHandler OnSpatialButtonUpEvent { get; set; }
		SpatialButtonClickHandler OnSpatialButtonClickEvent { get; set; }

		SpatialButton SpatialButtonDown { get; }
		SpatialButton SpatialButtonUnderCursor { get; }
		SpatialButton PreviousSpatialButtonUnderCursor { get; }
		bool IsPointerOverPressedSpatialButton { get; }
		bool IsPointerOverSpatialButton { get; }

		float PointerAxisX { get; }
		float PointerAxisY { get; }
		float PointerPositionX { get; }
		float PointerPositionY { get; }
		bool IsPointerOverUIObject { get; }
		bool IsAnyObjectSelectedAndHolding { get; }
		Vector2 PointerPosition { get; }
	}
}
