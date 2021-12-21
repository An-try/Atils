using UnityEngine;

namespace Atils.Runtime.Inputs
{
	public class InputService : MonoBehaviour
	{
		protected virtual SpatialButton DetectSpatialButtonUnderCursor()
		{
			Ray ray = UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				return hit.transform.GetComponent<SpatialButton>();
			}
			else
			{
				return null;
			}
		}

		protected virtual SpatialButton DetectSpatialButtonEnterOrExit(
			SpatialButton spatialButtonUnderCursor,
			SpatialButton previousSpatialButtonUnderCursor,
			SpatialButtonEnterHandler onSpatialButtonEnterEvent,
			SpatialButtonExitHandler onSpatialButtonExitEvent)
		{
			if (spatialButtonUnderCursor != previousSpatialButtonUnderCursor)
			{
				if (previousSpatialButtonUnderCursor != null)
				{
					previousSpatialButtonUnderCursor.OnExit();
					onSpatialButtonExitEvent?.Invoke(previousSpatialButtonUnderCursor);
				}

				if (spatialButtonUnderCursor != null)
				{
					spatialButtonUnderCursor.OnEnter();
					onSpatialButtonEnterEvent?.Invoke(spatialButtonUnderCursor);
				}

				previousSpatialButtonUnderCursor = spatialButtonUnderCursor;
			}

			return previousSpatialButtonUnderCursor;
		}
	}
}
