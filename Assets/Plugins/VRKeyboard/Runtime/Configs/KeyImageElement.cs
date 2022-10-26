using UnityEngine;
using UnityEngine.UIElements;

public class KeyImageElement : Image
{
	public KeyImageElement(KeyImageElement keyImageElement)
	{
		for (int i = 0; i < keyImageElement.styleSheets.count; i++)
		{
			this.styleSheets.Add(keyImageElement.styleSheets[i]);
		}

		this.image = keyImageElement.image;
		this.pickingMode = keyImageElement.pickingMode;
	}

	public KeyImageElement(StyleSheet styleSheet, Sprite sprite, PickingMode pickingMode)
	{
		this.styleSheets.Add(styleSheet);
		this.style.backgroundImage = sprite.ToTexture2D();
		this.pickingMode = pickingMode;
	}
}
