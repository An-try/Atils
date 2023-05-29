using UnityEngine;

namespace Atils.Runtime.Utils
{
	public class LookAtMainCamera : MonoBehaviour
	{
		[SerializeField] private bool _canRotate = true;

		private Transform _mainCamera = default;

		private void Start()
		{
			_mainCamera = UnityEngine.Camera.main.transform;

			if (_mainCamera == null)
			{
				Debug.LogError(typeof(LookAtMainCamera) + ": Main Camera does not exist.");
			}
		}

		private void Update()
		{
			if (_canRotate && _mainCamera != null)
			{
				transform.rotation = _mainCamera.rotation;
			}
		}
	}
}
