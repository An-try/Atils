namespace Atils.Runtime.Inputs
{
	public interface IInputService
	{
		PointerHoldHandler OnPointerHoldEvent { get; set; }
		PointerScaleHandler OnPointerScaleEvent { get; set; }
		SpatialButtonEnterHandler OnSpatialButtonEnterEvent { get; set; }
		SpatialButtonExitHandler OnSpatialButtonExitEvent { get; set; }
		SpatialButtonClickHandler OnSpatialButtonClickEvent { get; set; }

		float PointerPositionX { get; }
		float PointerPositionY { get; }
		bool IsPointerOverGameObject { get; }
		SpatialButton SpatialButtonUnderCursor { get; }
		SpatialButton PreviousSpatialButtonUnderCursor { get; }
	}
}
