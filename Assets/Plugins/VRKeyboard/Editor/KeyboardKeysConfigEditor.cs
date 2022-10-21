using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(KeyboardKeysConfig))]
public class KeyboardKeysConfigEditor : UnityEditor.Editor
{
	private KeyboardKeysConfig _config => (KeyboardKeysConfig)target;
	private VisualElement _root = default;
	private StyleSheet _styleSheet = default;
	private DragAndDropManipulator _manipulator = default;

	private SerializedProperty _keyboardRowsHeightSerializedProperty = default;
	private IntegerField _keyboardRowsHeightInputField = default;

	private Button _clearButton = default;
	private Button _addKeyButton = default;
	private Button _addRowButton = default;
	private ToggleButton _toggleEditModeButton = default;

	private bool _keyEditMode = false;

	private void OnEnable()
	{
		_config.OnRowsUpdatedEvent += OnRowsUpdated;
	}

	private void OnDisable()
	{
		_config.OnRowsUpdatedEvent -= OnRowsUpdated;
	}

	private void OnRowsUpdated(List<RowData> rowDatas)
	{
		RebuildKeyboardElement(rowDatas, _root.Q<KeyboardElement>());
		RebuildDragAndDropManipulator();
		ToggleInputFields(_toggleEditModeButton.IsToggled);

		serializedObject.ApplyModifiedProperties();
		EditorUtility.SetDirty(_config);
	}

	#region Base

	public override VisualElement CreateInspectorGUI()
	{
		FindProperties();
		InitializeEditor();
		Compose();

		return _root;
	}

	private void FindProperties()
	{
		_keyboardRowsHeightSerializedProperty = serializedObject.FindProperty(nameof(KeyboardKeysConfig.KeyboardRowsHeight));
	}

	private void InitializeEditor()
	{
		_root = new VisualElement();
		_styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/VRKeyboard/Runtime/Configs/KeyboardKeysConfig.uss");

		_clearButton = new Button(ClearConfig);
		_clearButton.text = "Clear";

		_addKeyButton = new Button(AddKeyToConfig);
		_addKeyButton.text = "Add Key";

		_addRowButton = new Button(AddKeyToConfig);
		_addRowButton.text = "+";

		_toggleEditModeButton = new ToggleButton("Toggle edit mode (On)", "Toggle edit mode (Off)", _styleSheet, ToggleEditMode);

		_keyboardRowsHeightInputField = new IntegerField("Keyboard Rows Height");
		_keyboardRowsHeightInputField.BindProperty(_keyboardRowsHeightSerializedProperty);
		_keyboardRowsHeightInputField.RegisterValueChangedCallback(SetKeyboardRowsHeight);
	}

	private void SetKeyboardRowsHeight(ChangeEvent<int> output)
	{
		_root.Q<KeyboardElement>().SetRowsHeight(output.newValue);
	}

	private void Compose()
	{
		_root.Add(_clearButton);
		_root.Add(_addKeyButton);
		_root.Add(_toggleEditModeButton);

		_root.Add(_keyboardRowsHeightInputField);

		KeyboardElement keyboardElement = new KeyboardElement(_styleSheet);
		RebuildKeyboardElement(_config.Rows, keyboardElement);
		_root.Add(keyboardElement);

		RebuildDragAndDropManipulator();
		ToggleInputFields(_toggleEditModeButton.IsToggled);
	}

	#endregion

	#region Data from config

	private void ReadAndApplyDataFromConfig()
	{

	}

	private void ReadDataFromConfig()
	{

	}

	private void ApplyDataFromConfig()
	{

	}

	#endregion

	#region Data to config

	private void WriteDataToConfig()
	{

	}

	#endregion

	#region Key editing

	private void EnableKeyEditMode(KeyElement keyElement)
	{
		_root.Add(new KeyEditContainer(_styleSheet, keyElement.KeyData));
	}

	private void DisableKeyEditMode()
	{
		_root.Remove(_root.Q<KeyEditContainer>());
	}

	#endregion

	private void AddKeyToConfig()
	{
		_config.AddNewKey();
	}

	private void ClearConfig()
	{
		_config.Clear();
	}

	private void AddRowToConfig()
	{
		_config.AddNewRow();
	}

	public void AddKey(RowElement rowElement, KeyElement keyElement)
	{
		keyElement.OnPointerUpEvent += EnableKeyEditMode;
		rowElement.Add(keyElement);
	}

	private void RebuildKeyboardElement(List<RowData> rowDatas, KeyboardElement keyboardElement)
	{
		keyboardElement.Query<KeyElement>().ForEach(x => x.OnPointerUpEvent -= EnableKeyEditMode);
		keyboardElement.Clear();

		if (rowDatas == null)
		{
			return;
		}

		for (int i = 0; i < rowDatas.Count; i++)
		{
			RowElement rowElement = new RowElement(_styleSheet);

			for (int j = 0; j < rowDatas[i].Keys.Count; j++)
			{
				AddKey(rowElement, new KeyElement(_styleSheet, "0"));
			}

			keyboardElement.Add(rowElement);
		}
	}

	private void RebuildDragAndDropManipulator()
	{
		if (_manipulator != null)
		{
			_manipulator.OnKeyDroppedEvent -= OnKeyDropped;
			_manipulator.Dispose();
			_manipulator = null;
		}

		if (_toggleEditModeButton.IsToggled)
		{
			return;
		}

		UQueryBuilder<KeyElement> keyElements = _root.Query<KeyElement>();
		if (keyElements != null && keyElements.ToList().Count > 0)
		{
			_manipulator = new DragAndDropManipulator(_config, _root.Query<KeyboardElement>(), keyElements.ToList().Cast<VisualElement>().ToList());
			_manipulator.OnKeyDroppedEvent += OnKeyDropped;
		}
	}

	private void OnKeyDropped()
	{
		ToggleInputFields(_toggleEditModeButton.IsToggled);
		// update config
	}

	private void ToggleEditMode(bool isToggled)
	{
		RebuildDragAndDropManipulator();
		ToggleInputFields(isToggled);
	}

	private void ToggleInputFields(bool isToggled)
	{
		List<KeyElement> keyElementsList = _root.Query<KeyElement>().ToList();
		keyElementsList.ForEach(x => x.ToggleInputField(isToggled));
	}
}
