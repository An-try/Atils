using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using VRKeyboard.Runtime;

namespace VRKeyboard.Editor
{
	[CustomEditor(typeof(KeyboardKeysConfig))]
	public class KeyboardKeysConfigEditor : UnityEditor.Editor
	{
		private KeyboardKeysConfig _config => (KeyboardKeysConfig)target;
		private VisualElement _root = default;
		private StyleSheet _styleSheet = default;
		private DragAndDropManipulator _manipulator = default;

		//private SerializedObject _configSerializedObject = default;

		private SerializedProperty _keysSerializedProperty = default;
		private PropertyField _keysField = default;

		private SerializedProperty _testIntSerializedProperty = default;
		private IntegerField _testIntField = default;

		private Button _clearButton = default;
		private Button _addKeyButton = default;
		private ToggleButton _toggleEditModeButton = default;

		public override VisualElement CreateInspectorGUI()
		{
			FindProperties();
			InitializeEditor();
			Compose();

			UQueryBuilder<Key> allKeys = _root.Query<Key>();
			if (allKeys != null)
			{
				_manipulator = new DragAndDropManipulator(allKeys.ToList().Cast<VisualElement>().ToList(), _root.Query<Container>());
			}

			//// VisualElements objects can contain other VisualElement following a tree hierarchy.
			//VisualElement label = new Label("Hello World! From C#");
			//_root.Add(label);

			// Import UXML
			//var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/VRKeyboard/Runtime/Configs/KeyboardKeysConfig.uxml");
			//VisualElement labelFromUXML = visualTree.Instantiate();
			//root.Add(labelFromUXML);

			// A stylesheet can be added to a VisualElement.
			// The style will be applied to the VisualElement and all of its children.
			//StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/VRKeyboard/Runtime/Configs/KeyboardKeysConfig.uss");
			//VisualElement labelWithStyle = new Label("Hello World! With Style");
			//labelWithStyle.styleSheets.Add(styleSheet);
			//root.Add(labelWithStyle);

			return _root;
		}

		private void FindProperties()
		{
			//_configSerializedObject = new SerializedObject(_config);

			_keysSerializedProperty = serializedObject.FindProperty(nameof(KeyboardKeysConfig.Keys));
			_testIntSerializedProperty = serializedObject.FindProperty(nameof(KeyboardKeysConfig.TestInt));
		}

		private void InitializeEditor()
		{
			_root = new VisualElement();
			_styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/VRKeyboard/Runtime/Configs/KeyboardKeysConfig.uss");

			_clearButton = new Button(ClearConfig);
			_clearButton.text = "Clear";

			_addKeyButton = new Button(AddKeyToConfig);
			_addKeyButton.text = "Add Key";

			_toggleEditModeButton = new ToggleButton("Toggle edit mode", _styleSheet, ToggleEditMode);

			_keysField = new PropertyField(_keysSerializedProperty);

			_testIntField = new IntegerField();
			_testIntField.BindProperty(_testIntSerializedProperty);
		}

		private void Compose()
		{
			_root.Add(_clearButton);
			_root.Add(_addKeyButton);
			_root.Add(_toggleEditModeButton);

			_root.Add(_keysField);
			_root.Add(_testIntField);

			VisualElement container = new Container(_styleSheet);

			for (int i = 0; i < _config.Keys.Count; i++)
			{
				VisualElement row = new Row(_styleSheet);

				for (int j = 0; j < _config.Keys[i].Count; j++)
				{
					row.Add(new Key(_styleSheet, "0"));
				}

				container.Add(row);
			}

			_root.Add(container);
		}

		private void ClearConfig()
		{
			_root.Q<Container>().Clear();
			_config.Clear();

			UQueryBuilder<Key> allKeys = _root.Query<Key>();
			if (allKeys != null)
			{
				_manipulator = new DragAndDropManipulator(allKeys.ToList().Cast<VisualElement>().ToList(), _root.Query<Container>());
			}
		}

		private void AddKeyToConfig()
		{
			if (_config.Keys.Count <= 0)
			{
				_root.Q<Container>().Add(new Row(_styleSheet));
			}

			Key key = new Key(_styleSheet, "0");
			key.ToggleInputField(_toggleEditModeButton.IsToggled);

			VisualElement container = _root.Q<Container>();
			container[container.childCount - 1].Add(key);

			_config.AddKey();

			UQueryBuilder<Key> allKeys = _root.Query<Key>();
			if (allKeys != null)
			{
				_manipulator = new DragAndDropManipulator(allKeys.ToList().Cast<VisualElement>().ToList(), _root.Query<Container>());
			}
		}

		private void ToggleEditMode()
		{
			ToggleInputFields(_toggleEditModeButton.IsToggled);

			if (_toggleEditModeButton.IsToggled)
			{
				//_toggleEditModeButton.style.color = new StyleColor(new Color(50, 50, 50));
			}
			else
			{
				//_toggleEditModeButton.style.color = new StyleColor(new Color(196, 196, 196));
			}
		}

		private void ToggleInputFields(bool isToggled)
		{
			UQueryBuilder<Key> allKeys = _root.Query<Key>();
			List<Key> keys = allKeys.ToList();
			keys.ForEach(x => x.ToggleInputField(isToggled));
		}
	}
}
