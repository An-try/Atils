using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropManipulator : IDisposable
{
	public Action OnKeyElementRemovedEvent { get; set; }
	public Action OnKeyDroppedEvent { get; set; }

	private KeyboardKeysConfig _config { get; } = default;
	private KeyboardElement _keyboardElement { get; } = default;
	private List<VisualElement> _allTargets { get; } = default;

	private VisualElement _target { get; set; } = default;
	private int _previousRowIndex { get; set; } = default;
	private int _previousKeyIndex { get; set; } = default;
	private Vector2 _targetStartPosition { get; set; } = default;
	private Vector3 _pointerStartPosition { get; set; } = default;
	private bool _canDrag { get; set; } = false;

	public DragAndDropManipulator(KeyboardKeysConfig config, KeyboardElement keyboardElement, List<VisualElement> targets)
	{
		_config = config;
		_keyboardElement = keyboardElement;
		_allTargets = targets;

		RegisterCallbacksOnTarget();
	}

	// Registering when this.target is not null
	private void RegisterCallbacksOnTarget()
	{
		_allTargets.ForEach(x => x.RegisterCallback<PointerDownEvent>(PointerDownHandler));
		_allTargets.ForEach(x => x.RegisterCallback<PointerMoveEvent>(PointerMoveHandler));
		_allTargets.ForEach(x => x.RegisterCallback<PointerUpEvent>(PointerUpHandler));
		_allTargets.ForEach(x => x.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler));
	}

	// Unregistering when this.target is null
	private void UnregisterCallbacksFromTarget()
	{
		_allTargets.ForEach(x => x.UnregisterCallback<PointerDownEvent>(PointerDownHandler));
		_allTargets.ForEach(x => x.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler));
		_allTargets.ForEach(x => x.UnregisterCallback<PointerUpEvent>(PointerUpHandler));
		_allTargets.ForEach(x => x.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler));
	}

	private void PointerDownHandler(PointerDownEvent evt)
	{
		_target = (VisualElement)evt.target;

		//// Save some data about key element before dragging
		//_previousRowIndex = _keyboardElement.IndexOf(_target.parent);
		//_previousKeyIndex = _target.parent.IndexOf(_target);

		//// Extract a target from its hierarchy and add it to the top layer to draw it on top of other elements
		//float height = _target.layout.height;
		//float width = _target.layout.width;
		//_target.RemoveFromHierarchy();
		//_target.style.position = Position.Absolute;
		//_target.style.height = height;
		//_target.style.width = width;
		//_keyboardElement.Add(_target);

		//// Move target to the cursur
		//Vector3 cursorInKeyboardElementLocalPosition = _keyboardElement.TransformPoint(evt.position);
		//Vector3 targetOffset = new Vector3(_target.layout.width / 2, _target.layout.height / 2, 0);
		//_target.transform.position = cursorInKeyboardElementLocalPosition - targetOffset;

		// Save some values for target dragging calculations
		_targetStartPosition = _target.transform.position;
		_pointerStartPosition = evt.position;

		//_target.CapturePointer(evt.pointerId);

		//_canDrag = true;
		_canDrag = false;
	}

	private void PointerMoveHandler(PointerMoveEvent evt)
	{
		if (_target == null)
		{
			return;
		}

		Vector3 pointerDelta = evt.position - _pointerStartPosition;

		if (!_canDrag)
		{
			if (pointerDelta.magnitude > 5)
			{
				// Save some data about key element before dragging
				_previousRowIndex = _keyboardElement.IndexOf(_target.parent);
				_previousKeyIndex = _target.parent.IndexOf(_target);

				// Extract a target from its hierarchy and add it to the top layer to draw it on top of other elements
				float height = _target.layout.height;
				float width = _target.layout.width;
				_target.RemoveFromHierarchy();
				_target.style.position = Position.Absolute;
				_target.style.height = height;
				_target.style.width = width;
				SetVisualElementAndChildsAlpha(_target, 0.5f);
				_keyboardElement.Add(_target);

				// Move target to the cursur
				Vector3 cursorInKeyboardElementLocalPosition = _keyboardElement.TransformPoint(evt.position);
				Vector3 targetOffset = new Vector3(_target.layout.width / 2, _target.layout.height / 2, 0);
				_target.transform.position = cursorInKeyboardElementLocalPosition - targetOffset;

				// Save some values for target dragging calculations
				_targetStartPosition = _target.transform.position;
				_pointerStartPosition = evt.position;

				_target.CapturePointer(evt.pointerId);

				_canDrag = true;
			}
		}

		if (_canDrag && _target.HasPointerCapture(evt.pointerId))
		{
			_target.transform.position = new Vector2(
				Mathf.Clamp(_targetStartPosition.x + pointerDelta.x, 0, _keyboardElement.worldBound.width - _target.layout.width),
				Mathf.Clamp(_targetStartPosition.y + pointerDelta.y, 0, _keyboardElement.worldBound.height - _target.layout.height));
		}
	}

	private void PointerUpHandler(PointerUpEvent evt)
	{
		if (_canDrag && _target.HasPointerCapture(evt.pointerId))
		{
			_target.ReleasePointer(evt.pointerId);

			RowElement closestRowElement = GetRowUnderCursor(evt);
			KeyElement closestKeyElement = GetClosestKeyNearDroppedKey(closestRowElement, evt);
			DropKey(closestRowElement, closestKeyElement, evt);

			OnKeyDroppedEvent?.Invoke();
		}

		_target = null;
		_canDrag = false;
	}

	private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
	{ }

	private RowElement GetRowUnderCursor(PointerUpEvent evt)
	{
		List<RowElement> rowElementsList = _keyboardElement.Query<RowElement>().ToList();
		float cursorPositionInKeyboardElementY = _keyboardElement.TransformPoint(evt.position).y;
		float minDistance = float.MaxValue;
		RowElement closestRowElement = null;

		for (int i = 0; i < rowElementsList.Count; i++)
		{
			float rowElementCenterY = rowElementsList[i].layout.position.y + rowElementsList[i].layout.height / 2;
			float distance = Mathf.Abs(cursorPositionInKeyboardElementY - rowElementCenterY);

			if (distance < minDistance)
			{
				minDistance = distance;
				closestRowElement = rowElementsList[i];
			}
		}

		return closestRowElement;
	}

	private KeyElement GetClosestKeyNearDroppedKey(RowElement rowElement, PointerUpEvent evt)
	{
		List<KeyElement> keyElementsList = rowElement.Query<KeyElement>().ToList();

		if (keyElementsList.Count <= 0)
		{
			return null;
		}

		float cursorPositionInRowX = rowElement.TransformPoint(evt.position).x;
		float minDistance = float.MaxValue;
		KeyElement closestKeyElement = null;

		for (int i = 0; i < keyElementsList.Count; i++)
		{
			float keyCenterX = keyElementsList[i].layout.position.x + keyElementsList[i].layout.width / 2;
			float distance = Mathf.Abs(cursorPositionInRowX - keyCenterX);

			if (distance < minDistance)
			{
				minDistance = distance;
				closestKeyElement = keyElementsList[i];
			}
		}

		return closestKeyElement;
	}

	private void DropKey(RowElement rowElement, KeyElement closestKeyElement, PointerUpEvent evt)
	{
		int indexForDroppedKey = 0;

		if (closestKeyElement != null)
		{
			int indexOfClosestKeyElement = rowElement.IndexOf(closestKeyElement);
			indexForDroppedKey = indexOfClosestKeyElement;

			float cursorPositionRelativeToClosestKeyElementX = closestKeyElement.TransformPoint(evt.position).x;
			// If cursor is on the right side from the center of the nearest key
			if (cursorPositionRelativeToClosestKeyElementX > closestKeyElement.layout.width / 2)
			{
				indexForDroppedKey++;
			}
		}

		KeyData keyData = (_target as KeyElement).KeyData;
		int rowIndex = _keyboardElement.IndexOf(rowElement);

		_target.RemoveFromHierarchy();

		try
		{
			_config.RemoveKeyAt(_previousRowIndex, _previousKeyIndex);
			_config.AddKeyAt(keyData, rowIndex, indexForDroppedKey);
		}
		catch (Exception e)
		{
			Debug.LogError("Reselect the config");
			throw e;
		}
	}

	private void SetVisualElementAndChildsAlpha(VisualElement visualElement, float alpha)
	{
		Color backgroundColor = visualElement.resolvedStyle.backgroundColor;
		backgroundColor.a = alpha;
		visualElement.style.backgroundColor = backgroundColor;

		Color unityBackgroundImageTintColor = visualElement.resolvedStyle.unityBackgroundImageTintColor;
		unityBackgroundImageTintColor.a = alpha;
		visualElement.style.unityBackgroundImageTintColor = unityBackgroundImageTintColor;

		for (int i = 0; i < visualElement.childCount; i++)
		{
			SetVisualElementAndChildsAlpha(visualElement[i], alpha);
		}
	}

	public void Dispose()
	{
		UnregisterCallbacksFromTarget();
		_target = null;
	}
}
