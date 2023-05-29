using System;
using UnityEngine.UIElements;

public class KeyElement : VisualElement
{
	public Action<KeyElement> OnPointerUpEvent { get; set; }

	public KeyData KeyData { get; } = default;
	public KeyTextElement TextElement => this.Q<KeyTextElement>();
	public KeyImageElement ImageElement => this.Q<KeyImageElement>();

	public KeyElement(KeyElement keyElement)
	{
		for (int i = 0; i < keyElement.styleSheets.count; i++)
		{
			this.styleSheets.Add(keyElement.styleSheets[i]);
		}

		this.KeyData = keyElement.KeyData;

		if (keyElement.TextElement != null)
		{
			this.Add(new KeyTextElement(keyElement.TextElement));
		}
		if (keyElement.ImageElement != null)
		{
			this.Add(new KeyImageElement(keyElement.ImageElement));
		}

		this.style.width = KeyData.Width;

		this.RegisterCallback<PointerDownEvent>(OnPointerDown);
		this.RegisterCallback<PointerUpEvent>(OnPointerUp);
	}

	public KeyElement(StyleSheet styleSheet, KeyData keyData)
	{
		this.styleSheets.Add(styleSheet);

		this.KeyData = keyData;

		if (keyData.KeyType == KeyTypes.Text)
		{
			this.Add(new KeyTextElement(styleSheet, keyData.Text, PickingMode.Ignore));
		}
		else
		{
			this.Add(new KeyImageElement(styleSheet, keyData.Sprite, PickingMode.Ignore));
		}

		this.style.width = KeyData.Width;

		this.RegisterCallback<PointerDownEvent>(OnPointerDown);
		this.RegisterCallback<PointerUpEvent>(OnPointerUp);
	}

	private void OnPointerDown(PointerDownEvent evt)
	{ }

	private void OnPointerUp(PointerUpEvent evt)
	{
		OnPointerUpEvent?.Invoke(this);
	}
}
