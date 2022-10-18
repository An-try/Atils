using UnityEngine.UIElements;

public class Key : VisualElement
{
	public Key(StyleSheet styleSheet, string text)
	{
		this.styleSheets.Add(styleSheet);

		//this.Add(new KeyTextElement(styleSheet, text));
		this.Add(new KeyTextField(styleSheet, text));

		this.Q<KeyTextField>().RegisterCallback<ChangeEvent<string>>(OnTextChanged);
	}

	public void EnableInputField()
	{
		ToggleInputField(true);
	}

	public void DisableInputField()
	{
		ToggleInputField(false);
	}

	public void ToggleInputField()
	{
		ToggleInputField(!this.Q<KeyTextField>().enabledSelf);
	}

	public void ToggleInputField(bool isToggled)
	{
		this.Q<KeyTextField>().SetEnabled(isToggled);
	}

	private void OnTextChanged(ChangeEvent<string> changeEvent)
	{
		//this.Q<KeyTextElement>().text = changeEvent.newValue;
	}
}
