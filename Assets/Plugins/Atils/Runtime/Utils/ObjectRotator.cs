using Atils.Runtime.Inputs;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace EllanceChapter2.Runtime.Utils
{
	public class ObjectRotator : MonoBehaviour
	{
		public Action OnRotatedEvent { get; set; }

		[SerializeField] private bool _allowRotation = true;
		[SerializeField] private float _rotationSpeed = 100;
		[SerializeField, Range(0, 180)] private float _upRotationClamp = 90;
		[SerializeField, Range(0, 180)] private float _downRotationClamp = 90;
		[SerializeField, Range(0, 180)] private float _leftRotationClamp = 180;
		[SerializeField, Range(0, 180)] private float _rightRotationClamp = 180;

#if UNITY_EDITOR
		[Header("Debug Settings")]
		[SerializeField] private float _rotationSpeedDebug = 1000;
#endif

		private IInputService _inputService = default;

		private Quaternion _defaultRotation = default;

		private Coroutine _rotateToDefaultCoroutine = default;
		private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

		private float _defaultUpClamp = default;
		private float _defaultDownClamp = default;
		private float _defaultLeftClamp = default;
		private float _defaultRightClamp = default;

		public bool IsRotationAllowed => _allowRotation;

		[Inject]
		private void Construct(IInputService inputService)
		{
			_inputService = inputService;
		}

		private void Awake()
		{
			_defaultUpClamp = _upRotationClamp;
			_defaultDownClamp = _downRotationClamp;
			_defaultLeftClamp = _leftRotationClamp;
			_defaultRightClamp = _rightRotationClamp;
		}

		private void OnEnable()
		{
			_inputService.OnPointerHoldEvent += TryRotateObject;
		}

		private void OnDisable()
		{
			_inputService.OnPointerHoldEvent -= TryRotateObject;
		}

		private void Start()
		{
			_defaultRotation = transform.rotation;
		}

		public void AllowRotation(bool isAllowed)
		{
			_allowRotation = isAllowed;
		}

		public void ResetRotationClamps()
		{
			SetRotationClamps(_defaultUpClamp, _defaultDownClamp, _defaultLeftClamp, _defaultRightClamp);
		}

		public void SetRotationClamps(float upClamp, float downClamp, float leftClamp, float rightClamp)
		{
			_upRotationClamp = upClamp;
			_downRotationClamp = downClamp;
			_leftRotationClamp = leftClamp;
			_rightRotationClamp = rightClamp;
		}

		public void RotateToDefault()
		{
			KillCoroutine(ref _rotateToDefaultCoroutine);
			_rotateToDefaultCoroutine = StartCoroutine(RotateToDefaultCoroutine());
		}

		public void RotateObject(float pointerX, float pointerY, float timeStep, float multiplier = 1)
		{
			if ((transform.eulerAngles.y >= 0 && transform.eulerAngles.y <= 90) ||
				(transform.eulerAngles.y >= 270 && transform.eulerAngles.y <= 360))
			{
				pointerY *= -1;
			}

			float rotationSpeed = _rotationSpeed;

#if UNITY_EDITOR
			rotationSpeed = _rotationSpeedDebug;
#endif

			Vector3 additionalEulerAngles = new Vector3(-pointerY * rotationSpeed * multiplier * timeStep,
														-pointerX * rotationSpeed * multiplier * timeStep,
														0);

			float newEulerAngleX = transform.localEulerAngles.x + additionalEulerAngles.x;
			float newEulerAngleY = transform.localEulerAngles.y + additionalEulerAngles.y;
			float newEulerAngleZ = transform.localEulerAngles.z + additionalEulerAngles.z;

			if ((newEulerAngleX > 360 - _downRotationClamp && newEulerAngleX < 360 + _downRotationClamp) ||
				(newEulerAngleX > -_upRotationClamp && newEulerAngleX < _upRotationClamp))
			{

			}
			else
			{
				newEulerAngleX = transform.localEulerAngles.x;
			}

			if ((newEulerAngleY > 360 - _rightRotationClamp && newEulerAngleY < 360 + _rightRotationClamp) ||
				(newEulerAngleY > -_leftRotationClamp && newEulerAngleY < _leftRotationClamp))
			{

			}
			else
			{
				newEulerAngleY = transform.localEulerAngles.y;
			}

			Vector3 newEulerAngles = new Vector3(newEulerAngleX, newEulerAngleY, newEulerAngleZ);
			transform.localEulerAngles = newEulerAngles;
			OnRotatedEvent?.Invoke();
		}

		private void TryRotateObject(float pointerX, float pointerY, float timeStep, float multiplier = 1)
		{
			pointerX = _inputService.PointerAxisX;
			pointerY = _inputService.PointerAxisY;

			if (!_allowRotation ||
				_inputService.IsPointerOverPressedSpatialButton ||
				_inputService.IsPointerOverUIObject ||
				_inputService.IsAnyObjectSelectedAndHolding)
			{
				return;
			}

			RotateObject(pointerX, pointerY, timeStep, multiplier);
		}

		private void KillCoroutine(ref Coroutine coroutine)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		private IEnumerator RotateToDefaultCoroutine()
		{
			Quaternion currentRotation = transform.rotation;

			while (true)
			{
				transform.rotation = Quaternion.Lerp(currentRotation, _defaultRotation, Time.fixedDeltaTime);
				OnRotatedEvent?.Invoke();
				yield return _waitForFixedUpdate;
			}
		}
	}
}
