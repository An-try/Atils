using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyImageDataElement : KeyDataElement
{
	public KeyImageDataElement(StyleSheet styleSheet, KeyData keyData) : base(styleSheet, keyData)
	{ }

	protected override void CreateContent(StyleSheet styleSheet, KeyData keyData)
	{
		VisualElement keyImageDisplay = new VisualElement();
		keyImageDisplay.style.height = 50;
		keyImageDisplay.style.marginTop = 5;
		keyImageDisplay.style.marginLeft = 5;
		keyImageDisplay.style.marginRight = 5;
		keyImageDisplay.style.marginBottom = 5;
		keyImageDisplay.style.flexDirection = FlexDirection.Row;

		ObjectField objectField = new ObjectField();
		objectField.styleSheets.Add(styleSheet);
		objectField.style.width = 200;
		objectField.style.height = 20;
		objectField.objectType = typeof(Sprite);
		objectField.RegisterCallback<ChangeEvent<Object>>(OnKeySpriteChanged);
		objectField.value = keyData.Sprite;

		Image imageElement = new Image();
		imageElement.style.width = 50;
		imageElement.style.height = 50;
		imageElement.style.backgroundColor = Color.gray;
		imageElement.image = keyData.Sprite.ToTexture2D();

		keyImageDisplay.Add(imageElement);
		keyImageDisplay.Add(objectField);

		this.Add(keyImageDisplay);

		base.CreateContent(styleSheet, keyData);
	}

	private void OnKeySpriteChanged(ChangeEvent<Object> changeEvent)
	{
		Sprite sprite = (Sprite)changeEvent.newValue;
		KeyData.Sprite = sprite;
		this.Q<Image>().image = sprite.ToTexture2D();
	}
}
