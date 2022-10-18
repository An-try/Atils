using UnityEngine.UIElements;

public class Container : VisualElement
{
	public Container()
	{ }

	public Container(StyleSheet styleSheet)
	{
		styleSheets.Add(styleSheet);
	}
}
