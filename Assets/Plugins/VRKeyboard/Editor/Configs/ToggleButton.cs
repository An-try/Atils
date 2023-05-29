using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ToggleButton : Button
{
	public bool IsToggled { get; private set; } = false;

	private string _toggledOnText = default;
	private string _toggledOffText = default;
	private StyleColor _toggledOnTextColor = new StyleColor(Color.white);
	private StyleColor _toggledOffTextColor = default;
	private Action<bool> _onToggle = default;

	public ToggleButton(string text, StyleSheet styleSheet, Action<bool> onToggle)
	{
		this.text = text;
		this.styleSheets.Add(styleSheet);
		_onToggle += onToggle;

		this.clicked += OnClicked;
		_toggledOffTextColor = this.style.color;
		UpdateText();
	}

	public ToggleButton(string toggledOnText, string toggledOffText, StyleSheet styleSheet, Action<bool> onToggle)
	{
		_toggledOnText = toggledOnText;
		_toggledOffText = toggledOffText;
		this.styleSheets.Add(styleSheet);
		_onToggle += onToggle;

		this.clicked += OnClicked;
		_toggledOffTextColor = this.style.color;
		UpdateText();
	}

	private void OnClicked()
	{
		IsToggled = !IsToggled;
		_onToggle?.Invoke(IsToggled);

		UpdateText();
	}

	private void UpdateText()
	{
		if (IsToggled)
		{
			this.text = _toggledOnText;
			this.style.color = _toggledOnTextColor;
		}
		else
		{
			this.text = _toggledOffText;
			this.style.color = _toggledOffTextColor;
		}
	}
}
