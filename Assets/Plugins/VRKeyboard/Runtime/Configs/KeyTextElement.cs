using UnityEngine.UIElements;

public class KeyTextElement : TextElement
{
	public KeyTextElement(StyleSheet styleSheet, string text)
	{
		this.styleSheets.Add(styleSheet);
		this.text = text;
	}
}
