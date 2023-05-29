using UnityEngine.UIElements;

public class KeyTextField : TextField
{
	public KeyTextField(KeyTextField keyTextField)
	{
		for (int i = 0; i < keyTextField.styleSheets.count; i++)
		{
			this.styleSheets.Add(keyTextField.styleSheets[i]);
		}

		this.value = keyTextField.text;
	}

	public KeyTextField(StyleSheet styleSheet, string text)
	{
		this.styleSheets.Add(styleSheet);
		this.value = text;
	}
}
