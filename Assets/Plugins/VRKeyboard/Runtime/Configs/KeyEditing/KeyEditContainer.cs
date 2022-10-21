using System.ComponentModel;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyEditContainer : VisualElement
{
	private StyleSheet _styleSheet = default;
	private KeyData _keyData = default;

	public KeyEditContainer(StyleSheet styleSheet, KeyData keyData)
	{
		_styleSheet = styleSheet;
		_keyData = keyData;

		this.styleSheets.Add(_styleSheet);
		RebuildContent(_keyData);
	}

	private void RebuildContent(KeyData keyData)
	{
		DestroyContent();

		CreateMainDataContainer(keyData);
		CreateAdditionalContainer(keyData);
	}

	private void DestroyContent()
	{
		this.Clear();
	}

	private void CreateMainDataContainer(KeyData keyData)
	{
		switch (keyData.KeyType)
		{
			case KeyTypes.Text:
				CreateMainTextContainer(keyData);
				break;
			case KeyTypes.Image:
				CreateMainImageContainer(keyData);
				break;
			default:
				throw new InvalidEnumArgumentException(nameof(keyData.KeyType), (int)keyData.KeyType, typeof(KeyTypes));
		}
	}

	private void CreateMainTextContainer(KeyData keyData)
	{
		VisualElement mainTextContainer = new VisualElement();
		mainTextContainer.style.height = 25;
		mainTextContainer.style.marginTop = 5;
		mainTextContainer.style.marginLeft = 5;
		mainTextContainer.style.marginRight = 5;
		mainTextContainer.style.marginBottom = 5;
		mainTextContainer.style.flexDirection = FlexDirection.Row;

		Label textLabel = new Label("Text:");
		textLabel.styleSheets.Add(_styleSheet);
		textLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
		
		TextField textInputField = new TextField();
		textInputField.styleSheets.Add(_styleSheet);
		textInputField.style.minWidth = 25;
		textInputField.value = keyData.Text;

		mainTextContainer.Add(textLabel);
		mainTextContainer.Add(textInputField);

		this.Add(mainTextContainer);
	}

	private void CreateMainImageContainer(KeyData keyData)
	{
		VisualElement mainImageContainer = new VisualElement();
		mainImageContainer.style.height = 50;
		mainImageContainer.style.marginTop = 5;
		mainImageContainer.style.marginLeft = 5;
		mainImageContainer.style.marginRight = 5;
		mainImageContainer.style.marginBottom = 5;
		mainImageContainer.style.flexDirection = FlexDirection.Row;

		ObjectField objectField = new ObjectField();
		objectField.styleSheets.Add(_styleSheet);
		objectField.style.width = 200;
		objectField.style.height = 20;
		objectField.objectType = typeof(Sprite);
		//objectField.value = key.sprite;

		int width = 50;
		int height = 50;
		IMGUIContainer imageContainer = new IMGUIContainer(() => DrawMainImage(objectField, width, height));
		imageContainer.styleSheets.Add(_styleSheet);

		mainImageContainer.Add(imageContainer);
		mainImageContainer.Add(objectField);

		this.Add(mainImageContainer);
	}

	public void DrawMainImage(ObjectField spriteField, int width, int height)
	{
		Texture2D texture = new Texture2D(width, height);
		Object image = spriteField.value;

		if (image != null)
		{
			texture = AssetPreview.GetAssetPreview(image);
		}

		GUILayout.Label("", GUILayout.Width(width), GUILayout.Height(height));
		Rect freeRect = new Rect(0, 0, width, height);
		GUI.DrawTexture(freeRect, texture);
	}

	private void OnMainImageChanged(ChangeEvent<Sprite> sprite)
	{
		Debug.LogWarning("a");
		Texture2D texture2D = AssetPreview.GetAssetPreview(sprite.newValue);
		this.Q<IMGUIContainer>().style.backgroundImage = new StyleBackground(texture2D);
	}

	private void CreateAdditionalContainer(KeyData keyData)
	{
		VisualElement widthDisplay = new VisualElement();
		widthDisplay.style.height = 20;
		widthDisplay.style.marginTop = 5;
		widthDisplay.style.marginLeft = 5;
		widthDisplay.style.marginRight = 5;
		widthDisplay.style.marginBottom = 5;
		widthDisplay.style.flexDirection = FlexDirection.Row;

		Label widthLabel = new Label("Width:");
		widthLabel.styleSheets.Add(_styleSheet);
		widthLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
		widthDisplay.Add(widthLabel);

		FloatField widthInputField = new FloatField();
		widthInputField.styleSheets.Add(_styleSheet);
		widthInputField.style.minWidth = 50;
		widthInputField.value = keyData.Width;
		widthDisplay.Add(widthInputField);

		this.Add(widthDisplay);
	}
}
