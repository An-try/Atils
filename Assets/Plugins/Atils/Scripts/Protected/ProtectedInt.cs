using Atils.Runtime.Attributes;
using Newtonsoft.Json;
using System;
using System.Globalization;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class ProtectedInt
{
	[SerializeField, JsonProperty] private string _value = 0.ToString("X4");
	[SerializeField, JsonProperty] private string _offset = 0.ToString("X4");
#if UNITY_EDITOR
#pragma warning disable IDE0052 // Remove unread private members
	[SerializeField, JsonIgnore, ReadOnly] private int _real;
#pragma warning restore IDE0052 // Remove unread private members
#endif

	[JsonIgnore]
	public int Value =>
		int.Parse(_value, NumberStyles.HexNumber) -
		int.Parse(_offset, NumberStyles.HexNumber);

	public ProtectedInt()
	{
		Random random = new Random();
		_offset = random.Next(-999999, 999999).ToString("X4");
		_value = _offset;
#if UNITY_EDITOR
		_real = Value;
#endif
	}

	public ProtectedInt(int value)
	{
		Random random = new Random();
		_offset = random.Next(-999999, 999999).ToString("X4");
		_value = (value + int.Parse(_offset, NumberStyles.HexNumber)).ToString("X4");
#if UNITY_EDITOR
		_real = Value;
#endif
	}

	public override string ToString()
	{
		return Value.ToString();
	}

	public static ProtectedInt operator +(ProtectedInt value1, ProtectedInt value2)
	{
		return new ProtectedInt(value1.Value + value2.Value);
	}

	public static ProtectedInt operator -(ProtectedInt value1, ProtectedInt value2)
	{
		return new ProtectedInt(value1.Value - value2.Value);
	}
}
