using System;
using UnityEngine.UIElements;

public class ToggleButton : Button
{
	public bool IsToggled { get; private set; } = default;

	public ToggleButton(string text, StyleSheet styleSheet, Action clickEvent)
	{
		this.text = text;
		this.styleSheets.Add(styleSheet);
		this.clicked += clickEvent;
		this.clicked += OnClicked;
	}

	private void OnClicked()
	{
		IsToggled = !IsToggled;
	}
}
