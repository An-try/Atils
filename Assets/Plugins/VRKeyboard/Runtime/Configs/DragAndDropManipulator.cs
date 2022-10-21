using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropManipulator : PointerManipulator, IDisposable
{
	public Action OnKeyElementRemovedEvent { get; set; }
	public Action OnKeyDroppedEvent { get; set; }

	private KeyboardKeysConfig _config { get; } = default;
	private KeyboardElement _keyboardElement { get; } = default;
	private List<VisualElement> _allTargets { get; } = default;

	private int _previousRowIndex { get; set; } = default;
	private int _previousKeyIndex { get; set; } = default;
	private Vector2 _targetStartPosition { get; set; } = default;
	private Vector3 _pointerStartPosition { get; set; } = default;
	private bool _enabled { get; set; } = false;

	public DragAndDropManipulator(KeyboardKeysConfig config, KeyboardElement keyboardElement, List<VisualElement> targets)
	{
		_config = config;
		_keyboardElement = keyboardElement;
		_allTargets = targets;

		this.target = new VisualElement();
	}

	// Registering when this.target is not null
	protected override void RegisterCallbacksOnTarget()
	{
		_allTargets.ForEach(x => x.RegisterCallback<PointerDownEvent>(PointerDownHandler));
		_allTargets.ForEach(x => x.RegisterCallback<PointerMoveEvent>(PointerMoveHandler));
		_allTargets.ForEach(x => x.RegisterCallback<PointerUpEvent>(PointerUpHandler));
		_allTargets.ForEach(x => x.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler));
	}

	// Unregistering when this.target is null
	protected override void UnregisterCallbacksFromTarget()
	{
		_allTargets.ForEach(x => x.UnregisterCallback<PointerDownEvent>(PointerDownHandler));
		_allTargets.ForEach(x => x.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler));
		_allTargets.ForEach(x => x.UnregisterCallback<PointerUpEvent>(PointerUpHandler));
		_allTargets.ForEach(x => x.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler));
	}

	private void PointerDownHandler(PointerDownEvent evt)
	{
		target = (VisualElement)evt.target;

		// Save some data about key element before dragging
		_previousRowIndex = _keyboardElement.IndexOf(target.parent);
		_previousKeyIndex = target.parent.IndexOf(target);

		// Extract a target from its hierarchy and add it to the top layer to draw it on top of other elements
		target.RemoveFromHierarchy();
		target.style.position = Position.Absolute;
		_keyboardElement.Add(target);

		// Move target to the cursur
		Vector3 cursorInKeyboardElementLocalPosition = _keyboardElement.TransformPoint(evt.position);
		Vector3 targetOffset = new Vector3(target.layout.width / 2, target.layout.height / 2, 0);
		target.transform.position = cursorInKeyboardElementLocalPosition - targetOffset;

		// Save some values for target dragging calculations
		_targetStartPosition = target.transform.position;
		_pointerStartPosition = evt.position;
		target.CapturePointer(evt.pointerId);
		_enabled = true;
	}

	private void PointerMoveHandler(PointerMoveEvent evt)
	{
		if (_enabled && target.HasPointerCapture(evt.pointerId))
		{
			Vector3 pointerDelta = evt.position - _pointerStartPosition;

			target.transform.position = new Vector2(
				Mathf.Clamp(_targetStartPosition.x + pointerDelta.x, 0, _keyboardElement.worldBound.width - target.layout.width),
				Mathf.Clamp(_targetStartPosition.y + pointerDelta.y, 0, _keyboardElement.worldBound.height - target.layout.height));
		}
	}

	private void PointerUpHandler(PointerUpEvent evt)
	{
		if (_enabled && target.HasPointerCapture(evt.pointerId))
		{
			target.ReleasePointer(evt.pointerId);

			RowElement closestRowElement = GetRowUnderCursor(evt);
			KeyElement closestKeyElement = GetClosestKeyNearDroppedKey(closestRowElement, evt);
			DropKey(closestRowElement, closestKeyElement, evt);

			target = new VisualElement();
			_enabled = false;

			OnKeyDroppedEvent?.Invoke();
		}
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
		int indexOfClosestKeyElement = rowElement.IndexOf(closestKeyElement);
		int indexForDroppedKey = indexOfClosestKeyElement;

		float cursorPositionRelativeToClosestKeyElementX = closestKeyElement.TransformPoint(evt.position).x;
		// If cursor is on the right side from the center of the nearest key
		if (cursorPositionRelativeToClosestKeyElementX > closestKeyElement.layout.width / 2)
		{
			indexForDroppedKey++;
		}

		target.RemoveFromHierarchy();
		_config.RemoveKey(_previousRowIndex, _previousKeyIndex);
		_config.AddKey((target as KeyElement).KeyData, _keyboardElement.IndexOf(rowElement), indexForDroppedKey);
	}

	public void Dispose()
	{
		this.target = null;
	}
}
