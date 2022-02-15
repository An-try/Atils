using Atils.Runtime.Inputs;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EllanceChapter2.Runtime.Utils
{
	public class ObjectScaler : MonoBehaviour
	{
		public Action OnScaledEvent { get; set; }

		[SerializeField] private bool _allowScaling = true;
		[SerializeField] private float _scalingMultiplier = 1;
		[SerializeField] private Slider _slider = default;
		[SerializeField] private float _minScale = 0.2f;
		[SerializeField] private float _maxScale = 2;

#if UNITY_EDITOR
		[Header("Debug Settings")]
		[SerializeField] private float _scalingMultiplierEditor = 1;
#endif

		private IInputService _inputService = default;

		[Inject]
		private void Construct(IInputService inputService)
		{
			_inputService = inputService;
		}

		private void OnEnable()
		{
			_inputService.OnPointerScaleEvent += TryScaleObject;
			_slider?.onValueChanged.AddListener(ScaleObject);
		}

		private void OnDisable()
		{
			_inputService.OnPointerScaleEvent -= TryScaleObject;
			_slider?.onValueChanged.RemoveListener(ScaleObject);
		}

		public void AllowScaling(bool isAllowed)
		{
			_allowScaling = isAllowed;
		}

		public void UpdateSliderFillAmount()
		{
			if (_slider != null)
			{
				_slider.value = Normalize(transform.localScale.x, _minScale, _maxScale);
			}
		}

		private void TryScaleObject(float scaleAmount, float timeStep, float multiplier = 1)
		{
			if (!_allowScaling ||
				_inputService.IsPointerOverUIObject ||
				_inputService.IsAnyObjectSelectedAndHolding)
			{
				return;
			}

			ScaleObject(scaleAmount, timeStep, multiplier);
		}

		public void ScaleObject(float scaleAmount, float timeStep, float multiplier = 1)
		{
			float scaleSpeed = scaleAmount * _scalingMultiplier;

#if UNITY_EDITOR
			scaleSpeed = scaleAmount * _scalingMultiplierEditor;
#endif

			float newScaleX = transform.localScale.x + scaleSpeed * multiplier * timeStep * 10;
			float newScaleY = transform.localScale.x + scaleSpeed * multiplier * timeStep * 10;
			float newScaleZ = transform.localScale.x + scaleSpeed * multiplier * timeStep * 10;

			newScaleX = Mathf.Clamp(newScaleX, _minScale, _maxScale);
			newScaleY = Mathf.Clamp(newScaleY, _minScale, _maxScale);
			newScaleZ = Mathf.Clamp(newScaleZ, _minScale, _maxScale);

			transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);
			UpdateSliderFillAmount();
			OnScaledEvent?.Invoke();
		}

		private void ScaleObject(float scaleAmount)
		{
			if (!_allowScaling)
			{
				return;
			}

			float denormalizedScaleAmount = Denormalize(scaleAmount, _minScale, _maxScale);

			float newScaleX = denormalizedScaleAmount;
			float newScaleY = denormalizedScaleAmount;
			float newScaleZ = denormalizedScaleAmount;

			newScaleX = Mathf.Clamp(newScaleX, _minScale, _maxScale);
			newScaleY = Mathf.Clamp(newScaleY, _minScale, _maxScale);
			newScaleZ = Mathf.Clamp(newScaleZ, _minScale, _maxScale);

			transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);
		}

		private float Normalize(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}

		private float Denormalize(float value, float min, float max)
		{
			return value * (max - min) + min;
		}
	}
}
