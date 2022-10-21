using System;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyElement : VisualElement
{
	public Action<KeyElement> OnPointerUpEvent { get; set; }

	public KeyData KeyData { get; } = default;
	public KeyTextField TextElement => this.Q<KeyTextField>();

	public KeyElement(KeyElement keyElement)
	{
		for (int i = 0; i < keyElement.styleSheets.count; i++)
		{
			this.styleSheets.Add(keyElement.styleSheets[i]);
		}

		this.KeyData = keyElement.KeyData;

		this.Add(new KeyTextField(keyElement.TextElement));

		this.RegisterCallback<PointerDownEvent>(OnPointerDown);
		this.RegisterCallback<PointerUpEvent>(OnPointerUp);
	}

	public KeyElement(StyleSheet styleSheet, string text)
	{
		this.styleSheets.Add(styleSheet);

		this.Add(new KeyTextField(styleSheet, text));

		this.RegisterCallback<PointerDownEvent>(OnPointerDown);
		this.RegisterCallback<PointerUpEvent>(OnPointerUp);
	}

	public void SetHeight(float height)
	{
		this.style.height = height;
		TextElement.style.height = height;
	}

	public void EnableInputField()
	{
		ToggleInputField(true);
	}

	public void DisableInputField()
	{
		ToggleInputField(false);
	}

	public void ToggleInputField()
	{
		ToggleInputField(!this.Q<KeyTextField>().enabledSelf);
	}

	public void ToggleInputField(bool isToggled)
	{
		this.Q<KeyTextField>().SetEnabled(isToggled);
	}

	private void OnPointerDown(PointerDownEvent evt)
	{ }

	private void OnPointerUp(PointerUpEvent evt)
	{
		OnPointerUpEvent?.Invoke(this);
	}
}
