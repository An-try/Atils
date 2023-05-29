using System;
using System.ComponentModel;
using UnityEngine.UIElements;

public class KeyEditContainer : VisualElement
{
	public Action<KeyData> OnKeyAddEvent { get; set; }

	private StyleSheet _styleSheet = default;

	public KeyEditContainer(StyleSheet styleSheet)
	{
		_styleSheet = styleSheet;

		this.styleSheets.Add(_styleSheet);

		AddNewKeyButton();
	}

	public KeyEditContainer(StyleSheet styleSheet, KeyData keyData)
	{
		_styleSheet = styleSheet;

		this.styleSheets.Add(_styleSheet);

		RebuildEditContainer(keyData);
	}

	private void OnKeyDataChanged(KeyData keyData)
	{
		RebuildEditContainer(keyData);
	}

	private void OnAddButtonClicked()
	{
		KeyData keyData = this.Q<KeyDataElement>().KeyData;
		
		Clear();
		AddNewKeyButton();

		OnKeyAddEvent?.Invoke(keyData);
	}

	private void OnCancelButtonClicked()
	{
		Clear();
		AddNewKeyButton();
	}

	private void AddNewKeyButton()
	{
		this.Add(new TextButton(_styleSheet, "New key", OnNewKeyButtonClicked));
	}

	private void OnNewKeyButtonClicked()
	{
		RebuildEditContainer(new KeyData());
	}

	private void RebuildEditContainer(KeyData keyData)
	{
		Clear();
		CreateKeyContent(keyData);
		CreateBottomButtons();
	}

	private void CreateKeyContent(KeyData keyData)
	{
		switch (keyData.KeyType)
		{
			case KeyTypes.Text:
				CreateKeyTextDataElement(keyData);
				break;
			case KeyTypes.Image:
				CreateKeyImageDataElement(keyData);
				break;
			default:
				throw new InvalidEnumArgumentException(nameof(keyData.KeyType), (int)keyData.KeyType, typeof(KeyTypes));
		}
	}

	private void CreateBottomButtons()
	{
		VisualElement bottomButtonsContainer = new VisualElement();
		bottomButtonsContainer.style.flexDirection = FlexDirection.Row;

		TextButton addButton = CreateButtomTextButton("Add", 50, OnAddButtonClicked);
		TextButton cancelButton = CreateButtomTextButton("Cancel", 50, OnCancelButtonClicked);

		bottomButtonsContainer.Add(addButton);
		bottomButtonsContainer.Add(cancelButton);

		this.Add(bottomButtonsContainer);
	}

	private void CreateKeyTextDataElement(KeyData keyData)
	{
		KeyTextDataElement keyTextDataElement = new KeyTextDataElement(_styleSheet, keyData);
		keyTextDataElement.OnKeyTypeChangedEvent += OnKeyDataChanged;
		this.Add(keyTextDataElement);
	}

	private void CreateKeyImageDataElement(KeyData keyData)
	{
		KeyImageDataElement keyImageDataElement = new KeyImageDataElement(_styleSheet, keyData);
		keyImageDataElement.OnKeyTypeChangedEvent += OnKeyDataChanged;
		this.Add(keyImageDataElement);
	}

	private TextButton CreateButtomTextButton(string text, float widthPercent, Action onClick)
	{
		TextButton bottomButton = new TextButton(_styleSheet, text, onClick);
		StyleLength styleLength = bottomButton.style.width;
		Length length = Length.Percent(widthPercent);
		styleLength.value = length;
		bottomButton.style.width = styleLength;

		return bottomButton;
	}
}
