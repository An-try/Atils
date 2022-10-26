using System;
using UnityEngine.UIElements;

public class TextButton : Button
{
	public TextButton(StyleSheet styleSheet, string text, Action onClick)
	{
		this.styleSheets.Add(styleSheet);
		this.text = text;
		this.clicked += onClick;
	}
}
