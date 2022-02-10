namespace Atils.Runtime.Inputs
{
	public delegate void PointerHoldHandler(float positionX, float positionY, float timeStep, float multiplier = 1);
	public delegate void PointerScaleHandler(float scaleAmount, float timeStep, float multiplier = 1);
	public delegate void SpatialButtonEnterHandler(SpatialButton spatialButton);
	public delegate void SpatialButtonExitHandler(SpatialButton spatialButton);
	public delegate void SpatialButtonClickHandler(SpatialButton spatialButton);
}