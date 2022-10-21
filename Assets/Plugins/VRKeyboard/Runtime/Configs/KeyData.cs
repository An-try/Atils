using System;
using UnityEngine;

[Serializable]
public class KeyData
{
	private KeyTypes _keyType = DefaultValues.KEY_TYPE;
	private string _text = DefaultValues.KEY_TEXT;
	private Sprite _sprite = DefaultValues.KEY_SPRITE;
	private int _width = DefaultValues.KEY_WIDTH;

	public KeyTypes KeyType => _keyType;
	public string Text => _text;
	public Sprite Sprite => _sprite;
	public int Width => _width;

	public KeyData()
	{ }
}
