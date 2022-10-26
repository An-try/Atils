using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyDataElement : VisualElement
{
	public Action<KeyData> OnKeyTypeChangedEvent { get; set; }

	public KeyData KeyData { get; private set; }

	public KeyDataElement(StyleSheet styleSheet, KeyData keyData)
	{
		this.styleSheets.Add(styleSheet);
		KeyData = keyData;

		CreateContent(styleSheet, KeyData);
	}

	protected virtual void CreateContent(StyleSheet styleSheet, KeyData keyData)
	{
		CreateKeyTypeDisplay(styleSheet, keyData);
		CreateWidthDisplay(styleSheet, keyData);
	}

	private void CreateKeyTypeDisplay(StyleSheet styleSheet, KeyData keyData)
	{
		VisualElement keyTypeDisplay = new VisualElement();
		keyTypeDisplay.style.height = 20;
		keyTypeDisplay.style.marginTop = 5;
		keyTypeDisplay.style.marginLeft = 5;
		keyTypeDisplay.style.marginRight = 5;
		keyTypeDisplay.style.marginBottom = 5;
		keyTypeDisplay.style.flexDirection = FlexDirection.Row;

		Label keyTypeLabel = new Label("Type:");
		keyTypeLabel.styleSheets.Add(styleSheet);
		keyTypeLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
		keyTypeDisplay.Add(keyTypeLabel);

		EnumField keyTypeInputField = new EnumField(DefaultValues.KEY_TYPE);
		keyTypeInputField.styleSheets.Add(styleSheet);
		keyTypeInputField.RegisterCallback<ChangeEvent<Enum>>(OnKeyTypeChanged);
		keyTypeInputField.value = keyData.KeyType;
		keyTypeDisplay.Add(keyTypeInputField);

		this.Add(keyTypeDisplay);
	}

	private void CreateWidthDisplay(StyleSheet styleSheet, KeyData keyData)
	{
		VisualElement widthDisplay = new VisualElement();
		widthDisplay.style.height = 20;
		widthDisplay.style.marginTop = 5;
		widthDisplay.style.marginLeft = 5;
		widthDisplay.style.marginRight = 5;
		widthDisplay.style.marginBottom = 5;
		widthDisplay.style.flexDirection = FlexDirection.Row;

		Label widthLabel = new Label("Width:");
		widthLabel.styleSheets.Add(styleSheet);
		widthLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
		widthDisplay.Add(widthLabel);

		IntegerField widthInputField = new IntegerField();
		widthInputField.styleSheets.Add(styleSheet);
		widthInputField.style.minWidth = 50;
		widthInputField.RegisterCallback<ChangeEvent<int>>(OnKeyWidthChanged);
		widthInputField.value = keyData.Width;
		widthDisplay.Add(widthInputField);

		this.Add(widthDisplay);
	}

	private void OnKeyTypeChanged(ChangeEvent<Enum> changeEvent)
	{
		KeyData.KeyType = (KeyTypes)changeEvent.newValue;
		OnKeyTypeChangedEvent?.Invoke(KeyData);
	}

	private void OnKeyWidthChanged(ChangeEvent<int> changeEvent)
	{
		KeyData.Width = changeEvent.newValue;
	}
}
