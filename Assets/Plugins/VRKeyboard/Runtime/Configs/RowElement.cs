using UnityEngine.UIElements;

public class RowElement : VisualElement
{
	public RowElement()
	{ }

	public RowElement(StyleSheet styleSheet)
	{
		styleSheets.Add(styleSheet);
	}

	public void SetHeight(float height)
	{
		this.style.height = height;
		this.Query<KeyElement>().ForEach(x => x.SetHeight(height));
	}
}
