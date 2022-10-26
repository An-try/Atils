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
	public int KeyboardRowsHeight = DefaultValues.ROW_HEIGHT;

	public void AddKeyAtEnd()
	{
		AddKeyAtEnd(new KeyData());
	}

	public void AddKeyAtEnd(KeyData keyData)
	{
		if (Rows == null || Rows.Count <= 0)
		{
			Rows = new List<RowData>();
			Rows.Add(new RowData(KeyboardRowsHeight));
		}

		Rows.Last().Keys.Add(keyData);

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void AddKeyAt(KeyData keyData, int rowIndex, int keyIndex)
	{
		if (Rows == null || Rows.Count <= 0)
		{
			AddKeyAtEnd(keyData);
			return;
		}

		if (rowIndex < 0 || rowIndex > Rows.Count)
		{
			throw new ArgumentOutOfRangeException();
		}

		if (keyIndex < 0 || keyIndex > Rows[rowIndex].Keys.Count)
		{
			throw new ArgumentOutOfRangeException();
		}

		Rows[rowIndex].Keys.Insert(keyIndex, keyData);

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void RemoveKeyAt(int rowIndex, int keyIndex, bool removeRowIfEmpty = false)
	{
		if (rowIndex < 0 || rowIndex >= Rows.Count ||
			keyIndex < 0 || keyIndex >= Rows[rowIndex].Keys.Count)
		{
		}

		Rows[rowIndex].Keys.RemoveAt(keyIndex);

		if (removeRowIfEmpty && Rows[rowIndex].Keys.Count <= 0)
		{
			RemoveRowAt(rowIndex);
		}

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void AddRowAtEnd()
	{
		AddRowAtEnd(new RowData(KeyboardRowsHeight));
	}

	public void AddRowAtEnd(RowData rowData)
	{
		if (Rows == null)
		{
			Rows = new List<RowData>();
		}

		Rows.Add(rowData);

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void RemoveRowAt(int rowIndex)
	{
		if (Rows == null || rowIndex < 0 || rowIndex >= Rows.Count)
		{
			return;
		}

		Rows.RemoveAt(rowIndex);

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void SetRowsHeight(int height)
	{
		KeyboardRowsHeight = height;
		Rows.ForEach(x => x.Height = KeyboardRowsHeight);

		OnRowsUpdatedEvent?.Invoke(Rows);
	}

	public void Clear()
	{
		Rows.Clear();
		Rows = new List<RowData>();

		OnRowsUpdatedEvent?.Invoke(Rows);
	}
}
