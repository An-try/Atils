using UnityEditor;
using UnityEngine;

public static class SpriteExtensions
{
	public static Texture2D ToTexture2D(this Sprite source)
	{
		return AssetPreview.GetAssetPreview(source);
	}
}
