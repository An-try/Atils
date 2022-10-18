using UnityEditor;
using UnityEditor.UIElements;
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

		//private SerializedObject _configSerializedObject = default;

		private SerializedProperty _keysSerializedProperty = default;
		private PropertyField _keysField = default;

		private SerializedProperty _testIntSerializedProperty = default;
		private IntegerField _testIntField = default;

		private Button _clearButton = default;
		private Button _addKeyButton = default;

		public override VisualElement CreateInspectorGUI()
		{
			FindProperties();
			InitializeEditor();
			Compose();

			Key key = _root.Q<Key>();
			if (key != null)
			{
				DragAndDropManipulator manipulator = new DragAndDropManipulator(key, _root);
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

			_clearButton = new Button(ClearConfig);
			_clearButton.text = "Clear";

			_addKeyButton = new Button(AddKeyToConfig);
			_addKeyButton.text = "Add Key";

			_keysField = new PropertyField(_keysSerializedProperty);

			_testIntField = new IntegerField();
			_testIntField.BindProperty(_testIntSerializedProperty);
		}

		private void Compose()
		{
			StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/VRKeyboard/Runtime/Configs/KeyboardKeysConfig.uss");

			_root.Add(_clearButton);
			_root.Add(_addKeyButton);

			_root.Add(_keysField);
			_root.Add(_testIntField);

			VisualElement slots = new Slots();

			for (int i = 0; i < _config.Keys.Count; i++)
			{
				VisualElement row = new Row();
				row.styleSheets.Add(styleSheet);

				for (int j = 0; j < _config.Keys[i].Count; j++)
				{
					VisualElement slot = new Slot();
					slot.styleSheets.Add(styleSheet);

					if (i == 0 && j == 0)
					{
						VisualElement key = new Key();
						key.styleSheets.Add(styleSheet);
						slot.Add(key);
					}

					row.Add(slot);
				}

				slots.Add(row);
			}

			_root.Add(slots);
		}

		private void ClearConfig()
		{
			_config.Clear();

			//Object[] objects = new Object[1] { target };
			//this.ResetTarget();
			//this.Repaint();
			//this.ReloadPreviewInstances();

			//serializedObject.ApplyModifiedProperties();
		}

		private void AddKeyToConfig()
		{
			_config.AddKey();

			//Object[] objects = new Object[1] { target };
			//this.ResetTarget();
			//this.Repaint();
			//this.ReloadPreviewInstances();

			//serializedObject.ApplyModifiedProperties();
		}
	}
}
