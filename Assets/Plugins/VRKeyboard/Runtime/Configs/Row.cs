using UnityEngine.UIElements;

public class Row : VisualElement
{
	public Row()
	{ }

	public Row(StyleSheet styleSheet)
	{
		styleSheets.Add(styleSheet);
	}
}
