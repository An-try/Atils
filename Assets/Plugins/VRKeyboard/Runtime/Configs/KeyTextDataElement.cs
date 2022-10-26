using UnityEngine;
using UnityEngine.UIElements;

public class KeyTextDataElement : KeyDataElement
{
	public KeyTextDataElement(StyleSheet styleSheet, KeyData keyData) : base(styleSheet, keyData)
	{ }

	protected override void CreateContent(StyleSheet styleSheet, KeyData keyData)
	{
		VisualElement keyTextDisplay = new VisualElement();
		keyTextDisplay.style.height = 20;
		keyTextDisplay.style.marginTop = 5;
		keyTextDisplay.style.marginLeft = 5;
		keyTextDisplay.style.marginRight = 5;
		keyTextDisplay.style.marginBottom = 5;
		keyTextDisplay.style.flexDirection = FlexDirection.Row;

		Label textLabel = new Label("Text:");
		textLabel.styleSheets.Add(styleSheet);
		textLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

		TextField textInputField = new TextField();
		textInputField.styleSheets.Add(styleSheet);
		textInputField.style.minWidth = 25;
		textInputField.RegisterCallback<ChangeEvent<string>>(OnKeyTextChanged);
		textInputField.value = keyData.Text;

		keyTextDisplay.Add(textLabel);
		keyTextDisplay.Add(textInputField);

		this.Add(keyTextDisplay);

		base.CreateContent(styleSheet, keyData);
	}

	private void OnKeyTextChanged(ChangeEvent<string> changeEvent)
	{
		KeyData.Text = changeEvent.newValue;
	}
}
