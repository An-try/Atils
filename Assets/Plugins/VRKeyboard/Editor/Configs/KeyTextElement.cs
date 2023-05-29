using UnityEngine.UIElements;

public class KeyTextElement : TextElement
{
	public KeyTextElement(KeyTextElement keyTextElement)
	{
		for (int i = 0; i < keyTextElement.styleSheets.count; i++)
		{
			this.styleSheets.Add(keyTextElement.styleSheets[i]);
		}

		this.text = keyTextElement.text;
		this.pickingMode = keyTextElement.pickingMode;
	}

	public KeyTextElement(StyleSheet styleSheet, string text, PickingMode pickingMode)
	{
		this.styleSheets.Add(styleSheet);
		this.text = text;
		this.pickingMode = pickingMode;
	}
}
