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

	private TextButton _clearButton = default;
	private TextButton _addRowButton = default;

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

		_clearButton = new TextButton(_styleSheet, "Clear", ClearConfig);
		_addRowButton = new TextButton(_styleSheet, "Add Row", AddRowToConfig);

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

		_root.Add(_keyboardRowsHeightInputField);

		KeyboardElement keyboardElement = new KeyboardElement(_styleSheet);
		RebuildKeyboardElement(_config.Rows, keyboardElement);
		_root.Add(keyboardElement);

		_root.Add(_addRowButton);

		KeyEditContainer keyEditContainer = new KeyEditContainer(_styleSheet);
		keyEditContainer.OnKeyAddEvent += AddKeyToConfig;
		_root.Add(keyEditContainer);

		RebuildDragAndDropManipulator();
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
		//_root.Add(new KeyEditContainer(_styleSheet, keyElement.KeyData));
	}

	private void DisableKeyEditMode()
	{
		_root.Remove(_root.Q<KeyEditContainer>());
	}

	#endregion

	private void ClearConfig()
	{
		_config.Clear();
	}

	private void AddKeyToConfig(KeyData keyData)
	{
		_config.AddKeyAtEnd(keyData);
	}

	private void AddRowToConfig()
	{
		_config.AddRowAtEnd();
	}

	private void RebuildKeyboardElement(List<RowData> rowDatas, KeyboardElement keyboardElement)
	{
		ClearKeyboardElement(keyboardElement);

		if (rowDatas == null)
		{
			return;
		}

		for (int i = 0; i < rowDatas.Count; i++)
		{
			RowElement rowElement = new RowElement(_styleSheet, rowDatas[i]);

			for (int j = 0; j < rowDatas[i].Keys.Count; j++)
			{
				AddKeyElement(rowElement, new KeyElement(_styleSheet, rowDatas[i].Keys[j]));
			}

			keyboardElement.Add(rowElement);
		}
	}

	private void ClearKeyboardElement(KeyboardElement keyboardElement)
	{
		keyboardElement.Query<KeyElement>().ForEach(x => x.OnPointerUpEvent -= EnableKeyEditMode);
		keyboardElement.Clear();
	}

	private void AddKeyElement(RowElement rowElement, KeyElement keyElement)
	{
		keyElement.OnPointerUpEvent += EnableKeyEditMode;
		rowElement.Add(keyElement);
	}

	private void RebuildDragAndDropManipulator()
	{
		if (_manipulator != null)
		{
			_manipulator.OnKeyDroppedEvent -= OnKeyDropped;
			_manipulator.Dispose();
			_manipulator = null;
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
		// update config
	}
}
