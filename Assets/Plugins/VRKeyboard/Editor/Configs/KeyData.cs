using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class KeyData
{
	public KeyTypes KeyType = DefaultValues.KEY_TYPE;
	public string Text = DefaultValues.KEY_TEXT;
	public string SpriteGUID = DefaultValues.KEY_SPRITE_GUID;
	public int Width = DefaultValues.KEY_WIDTH;

	public Sprite Sprite
	{
		get
		{
			if (string.IsNullOrEmpty(SpriteGUID))
			{
				return null;
			}

			string assetPath = AssetDatabase.GUIDToAssetPath(SpriteGUID);
			if (string.IsNullOrEmpty(assetPath))
			{
				return null;
			}

			UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));
			if (obj == null || !(obj is Sprite))
			{
				return null;
			}

			return (Sprite)obj;
		}
		set
		{
			if (value == null)
			{
				SpriteGUID = string.Empty;
				return;
			}

			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(value, out string guid, out long localId))
			{
				SpriteGUID = guid;
			}
			else
			{
				Debug.LogError("Unable to get object GUID and LocalFileIdentifier");
			}
		}
	}

	public KeyData()
	{ }
}
