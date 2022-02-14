using UnityEngine;
using UnityEngine.Events;

namespace Atils.Runtime.Inputs
{
	public class SpatialButton : MonoBehaviour
	{
		[SerializeField] private UnityEvent OnShowEvent = default;
		[SerializeField] private UnityEvent OnHideEvent = default;
		[SerializeField] private UnityEvent OnButtonEnterEvent = default;
		[SerializeField] private UnityEvent OnButtonExitEvent = default;
		[SerializeField] private UnityEvent OnButtonDownEvent = default;
		[SerializeField] private UnityEvent OnButtonUpEvent = default;
		[SerializeField] private UnityEvent OnButtonClickEvent = default;

		private bool _isShown = true;

		public void Show()
		{
			if (!_isShown)
			{
				OnShowEvent?.Invoke();
				_isShown = true;
			}
		}

		public void Hide()
		{
			if (_isShown)
			{
				OnHideEvent?.Invoke();
				_isShown = false;
			}
		}

		public void OnEnter()
		{
			if (_isShown)
			{
				OnButtonEnterEvent?.Invoke();
			}
		}

		public void OnExit()
		{
			if (_isShown)
			{
				OnButtonExitEvent?.Invoke();
			}
		}

		public void OnDown()
		{
			if (_isShown)
			{
				OnButtonDownEvent?.Invoke();
			}
		}

		public void OnUp()
		{
			if (_isShown)
			{
				OnButtonUpEvent?.Invoke();
			}
		}

		public void OnClick()
		{
			if (_isShown)
			{
				OnButtonClickEvent?.Invoke();
			}
		}
	}
}
