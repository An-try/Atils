using System;
using System.Collections.Generic;

[Serializable]
public class RowData
{
	public int Height = DefaultValues.ROW_HEIGHT;
	public List<KeyData> Keys = new List<KeyData>();

	public RowData(int height)
	{
		Height = height;
	}
}
