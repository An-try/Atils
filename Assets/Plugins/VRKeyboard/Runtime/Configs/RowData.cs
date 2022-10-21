using System;
using System.Collections.Generic;

[Serializable]
public class RowData
{
	private int _height = DefaultValues.ROW_HEIGHT;
	private List<KeyData> _keys = new List<KeyData>();

	public int Height => _height;
	public List<KeyData> Keys => _keys;

	public RowData()
	{ }
}
