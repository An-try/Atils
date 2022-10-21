using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// paths: KeyboardKeysConfigEditor, KeyboardKeysConfig.uxml

[CreateAssetMenu(fileName = "KeyboardKeysConfig", menuName = "VRKeyboard/KeyboardKeysConfig")]
public class KeyboardKeysConfig : ScriptableObject
{
	public Action<List<RowData>> OnRowsUpdatedEvent { get; set; }

	public List<RowData> Rows = new List<RowData>();
	// additional array for the new keys?
	public int KeyboardRowsHeight = 40;

	public void AddNewKey()
	{
		if (Rows == null || Rows.Count <= 0)
		{
			Rows = new List<RowData>();
			Rows.Add(new RowData());
		}

		if (Rows.Last().Keys.Count >= 4)
		{
			Rows.Add(new RowData());
		}

		Rows.Last().Keys.Add(new KeyData());

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void AddKey(KeyData keyData, int rowIndex, int keyIndex)
	{
		Rows[rowIndex].Keys.Insert(keyIndex, keyData);

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void RemoveKey(int rowIndex, int keyIndex, bool removeRowIfThereAreNoKeysLeft = true)
	{
		Rows[rowIndex].Keys.RemoveAt(keyIndex);

		if (removeRowIfThereAreNoKeysLeft && Rows[rowIndex].Keys.Count <= 0)
		{
			RemoveRow(rowIndex);
		}

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void AddNewRow()
	{
		if (Rows == null)
		{
			Rows = new List<RowData>();
		}

		Rows.Add(new RowData());

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void RemoveRow(int rowIndex)
	{
		if (Rows == null || rowIndex < 0 || rowIndex >= Rows.Count)
		{
			return;
		}

		Rows.RemoveAt(rowIndex);

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void Clear()
	{
		Rows.Clear();
		Rows = new List<RowData>();

		OnRowsUpdatedEvent?.Invoke(Rows);
	}
}
