using UnityEngine.UIElements;

public class KeyTextField : TextField
{
	public KeyTextField(StyleSheet styleSheet, string text)
	{
		this.styleSheets.Add(styleSheet);
		this.value = text;
	}
}
