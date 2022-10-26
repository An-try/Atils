using UnityEngine.UIElements;

public class RowElement : VisualElement
{
	public RowElement()
	{ }

	public RowElement(StyleSheet styleSheet, RowData rowData)
	{
		styleSheets.Add(styleSheet);
		SetHeight(rowData.Height);
	}

	public void SetHeight(float height)
	{
		this.style.height = height;
	}
}
