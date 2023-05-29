using UnityEngine.UIElements;

public class KeyboardElement : VisualElement
{
	public KeyboardElement()
	{ }

	public KeyboardElement(StyleSheet styleSheet)
	{
		styleSheets.Add(styleSheet);
	}

	public void SetRowsHeight(float height)
	{
		this.Query<RowElement>().ForEach(x => x.SetHeight(height));
	}
}
